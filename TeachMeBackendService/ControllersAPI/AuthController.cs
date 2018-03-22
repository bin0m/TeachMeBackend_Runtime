using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.Mobile.Server.Login;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Web.Http;
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

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
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
            var claimsPrincipal = User as ClaimsPrincipal;
            if (claimsPrincipal != null)
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
                await UserManager.SendEmailAsync(user.Id, "Reset Password", $"Please reset your password by using this {code}");
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
