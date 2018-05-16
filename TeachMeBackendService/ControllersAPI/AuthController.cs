using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.Mobile.Server.Login;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Web.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace TeachMeBackendService.ControllersAPI
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:ApiVersion}/auth")]
    [MobileAppController]
    public class AuthController : ApiController
    {
        private  ApplicationUserManager _userManager;
        private readonly string _signingKey;
        private readonly string _audience;
        private readonly string _issuer;
        private readonly int _jwtTokenExpirationTimeInHours;

        private const string WelcomeEmailSubject = "Welcome to TeachMe!";
        private const string WelcomeEmailBody = "Hi {0},\n Welcome to the Teachme application - where you can study and teach.\n Your username:{1} \n Good Luck!";

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        TeachMeBackendContext DbContext => Request.GetOwinContext().Get<TeachMeBackendContext>();
        private IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;

        public AuthController()
        {
            _signingKey = Environment.GetEnvironmentVariable("WEBSITE_AUTH_SIGNING_KEY");

            if (string.IsNullOrEmpty(_signingKey))
            {
                // WEBSITE_AUTH_SIGNING_KEY - is null, when it is run locally for debugging
                _signingKey = ConfigurationManager.AppSettings["SigningKey"];
                _audience = ConfigurationManager.AppSettings["ValidAudience"];
                _issuer = ConfigurationManager.AppSettings["ValidIssuer"];
            }
            else
            {
                var website = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");
                _audience = $"https://{website}/";
                _issuer = $"https://{website}/";
            }


            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["JwtTokenExpirationTimeInHours"]))
            {
                _jwtTokenExpirationTimeInHours = Int32.Parse(ConfigurationManager.AppSettings["JwtTokenExpirationTimeInHours"]);
            }
            else
            {
                _jwtTokenExpirationTimeInHours = 72;
            }
            
        }
                
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();

        }

        [Route("InternalUser", Name = "GetInternalUser")]
        public async Task<IHttpActionResult> GetInternalUser()
        {
            //var serviceUser = User as ClaimsPrincipal;
            //var ident = serviceUser?.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider").Value;
            //For Debug 
            var ident = "facebook";
            var newUser = new User();
            switch (ident)
            {
                case "facebook":
                    //"access_token": "EAAB6D5R20a0BAFKFcXCZAfiZB1TGyQCvn1RyQ6xEpooCYtX0mZBmhQbxUas58hGAsGbiZAijLya85YGoGAbSrCOgZA2lkQPLuft5ZAGbZBHxS2tWJGnCobibCVbxk66w6ZAyjOlF47PBoJhtmyZAAxDfnnv2CfL5H2osZD"
                    var token = Request.Headers.GetValues("X-MS-TOKEN-FACEBOOK-ACCESS-TOKEN").FirstOrDefault();
                    using (HttpClient client = new HttpClient())
                    {
                        using (HttpResponseMessage response = await client.GetAsync("https://graph.facebook.com/v3.0/me?fields=id,name,email" + "&access_token=" + token))
                        {
                            var o = JObject.Parse(await response.Content.ReadAsStringAsync());
                            newUser.FacebookId = o["id"].ToString();
                            newUser.Email = o["email"].ToString();
                            newUser.FullName = o["name"].ToString();                            
                            newUser.RegisterDate = DateTime.Now;
                            newUser.UserRole = UserRole.Student;
                            JToken birtday = o["birthday"];
                            if (birtday != null)
                            {
                                newUser.DateOfBirth = DateTime.Parse(birtday.ToString());
                            }
                            
                        }


                        // look for existing user with facebook Id
                        var existingUser = DbContext.Set<User>().SingleOrDefault(u => u.FacebookId == newUser.FacebookId);
                        if (existingUser != null)
                        {
                            // user have been registered already via facebook, return user
                            return Ok(existingUser);
                            //TODO: maybe create new local Auth token instead of automatic token created
                        }

                        // look for existing user with identical email adress
                        var userWithIdenticalEmail = await UserManager.FindByNameAsync(newUser.Email);
                        if (userWithIdenticalEmail != null)
                        {
                            //user already registered to the system, but not via facebook. Need to add Facebook to his account
                            var existingUser2 = DbContext.Set<User>().Find(userWithIdenticalEmail.Id);
                            if (existingUser2 != null)
                            {
                                existingUser2.FacebookId = newUser.FacebookId;
                                DbContext.SaveChanges();
                                return Ok(existingUser2);
                            }
                            else
                            {
                                //Flaw scenario: AppUser is exist, but inner User is not
                                //TODO: Anyway create inner user instead throwing an exception
                            }

                            //TODO: maybe create new local Auth token instead of automatic token created
                        }

                        //Create new local account for facebook user
                        var appUser = new ApplicationUser() { UserName = newUser.Email, Email = newUser.Email, FullName = newUser.FullName };

                        // get user's image from Facebook
                        using (HttpResponseMessage response = await client.GetAsync("https://graph.facebook.com/me" + "/picture?redirect=false&access_token=" + token))
                        {
                            var x = JObject.Parse(await response.Content.ReadAsStringAsync());
                            var image = (x["data"]["url"].ToString());
                            //TODO: Upload image to Azure Blob and get some blobPath
                            UploadImageToBlob(image);
                            newUser.AvatarPath = image;
                        }

                        try
                        {
                            IdentityResult result = await UserManager.CreateAsync(appUser);

                            if (!result.Succeeded)
                            {
                                return GetErrorResult(result);
                            }

                            await UserManager.AddToRoleAsync(appUser.Id, newUser.UserRole.ToString());

                            // Create new Internal User
                            DbContext.Set<User>().Add(newUser);
                            DbContext.SaveChanges();

                            //Send welcome email
                            await UserManager.SendEmailAsync(
                                appUser.Id,
                                WelcomeEmailSubject,
                                string.Format(WelcomeEmailBody, newUser.FullName, newUser.Login));

                            //TODO: maybe create new local Auth token instead of automatic token created
                            //var claims = new List<Claim>
                            //{
                            //    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                            //    new Claim(JwtRegisteredClaimNames.GivenName, appUser.FullName),
                            //    new Claim(ClaimTypes.PrimarySid, appUser.Id),
                            //    new Claim(ClaimTypes.Role, newUser.UserRole.ToString())
                            //};

                            //var newToken = AppServiceLoginHandler.CreateToken(claims, _signingKey, _audience, _issuer, TimeSpan.FromDays(_jwtTokenExpirationTimeInHours));

                            //loginResult = new LoginResult()
                            //{
                            //    AuthenticationToken = token.RawData,
                            //    User = user
                            //};
                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                            return BadRequest(message);
                        }              
                    }
                    break;
                case "google":
                    // Google case
                default:
                    // other identity provider
                    break;
            }
            return Ok(newUser);
        }

        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(UserManager.Users.ToList());
        }


        [Route("me")]
        [Authorize]
        public IHttpActionResult GetMyInfo()
        {
            var response = new ClaimsUserInfo();

            // Get the Claims of the current user.
            if (User is ClaimsPrincipal claimsPrincipal)
            {
                response.Sid = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
                //response.Sid = claimsPrincipal.FindFirst(ClaimTypes.GivenName).Value;
                response.Role = claimsPrincipal.FindFirst(ClaimTypes.Role).Value;
            }

            return Ok(response);
        }


        // POST api/v1.0/auth/Signup
        [AllowAnonymous]
        [Route("Signup")]
        public async Task<IHttpActionResult> Signup(UserSignUpModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LoginResult loginResult;

            var appUser = new ApplicationUser() { UserName = model.Email, Email = model.Email, FullName = model.FullName };            

            try
            {
                IdentityResult result = await UserManager.CreateAsync(appUser, model.Password);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                
                await UserManager.AddToRoleAsync(appUser.Id, model.Role.ToString());

                var user = new User
                {
                    Id = appUser.Id,
                    CompletedCoursesCount = 0,
                    Email = model.Email,
                    FullName = model.FullName,
                    Login = model.UserName,
                    RegisterDate = DateTime.Now,
                    AvatarPath = model.AvatarPath,
                    UserRole = model.Role,
                    DateOfBirth = model.DateOfBirth
                };

                DbContext.Set<User>().Add(user);
                DbContext.SaveChanges();

                //Send welcome email
                await UserManager.SendEmailAsync(
                    appUser.Id, 
                    WelcomeEmailSubject, 
                    string.Format(WelcomeEmailBody, model.FullName, model.UserName));
                
                // Create token for the new registered user
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, model.Email),
                    new Claim(JwtRegisteredClaimNames.GivenName, appUser.FullName),
                    new Claim(ClaimTypes.PrimarySid, appUser.Id),
                    new Claim(ClaimTypes.Role, model.Role.ToString())
                };

                var token = AppServiceLoginHandler.CreateToken(claims, _signingKey, _audience, _issuer, TimeSpan.FromDays(_jwtTokenExpirationTimeInHours));

                loginResult = new LoginResult()
                {
                    AuthenticationToken = token.RawData,
                    User = user
                };
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return BadRequest(message);
            }           

            return Ok(loginResult);
        }

        [AllowAnonymous]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(LoginViewModel model, string returnUrl = null)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LoginResult loginResult;

            try
            {
                var appUser = await UserManager.FindAsync(model.Email, model.Password);
                if (appUser == null)
                {
                    return Unauthorized();
                }

                loginResult = await ConstructLoginResult(appUser);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return BadRequest(message);
            }       

            return Ok(loginResult);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                // If user has to activate his email to confirm his account, the use code listing below
                //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                //{
                //    return Ok();
                //}
                if (user == null)
                {
                    return Ok();
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", $"Please reset your password by using this code: {code}");
                return Ok();
            }

            // If we got this far, something failed, redisplay form
            return BadRequest(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Ok();
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            return Ok();
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        private async Task<LoginResult> ConstructLoginResult(ApplicationUser appUser)
        {
            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, appUser.Email),
                    new Claim(JwtRegisteredClaimNames.GivenName, appUser.FullName),
                    new Claim(ClaimTypes.PrimarySid, appUser.Id)
                };

            var userRoles = await UserManager.GetRolesAsync(appUser.Id);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = AppServiceLoginHandler.CreateToken(claims, _signingKey, _audience, _issuer, TimeSpan.FromHours(_jwtTokenExpirationTimeInHours));

            var user = DbContext.Set<User>().Find(appUser.Id);

            return new LoginResult()
            {
                AuthenticationToken = token.RawData,
                User = user
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        private void UploadImageToBlob(string filePath)
        {
            string storageConnectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("images");

            var blockBlob = container.GetBlockBlobReference("anewblob");
            // Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = System.IO.File.OpenRead(filePath))
            {
                blockBlob.UploadFromStream(fileStream);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
