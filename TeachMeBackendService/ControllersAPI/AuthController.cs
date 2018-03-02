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
        private ApplicationUserManager _userManager;
        private string signingKey;
        private string audience;
        private string issuer;
        private int jwtTokenExpirationTimeInHours;

        TeachMeBackendContext dbContext
        {
            get
            {
                return Request.GetOwinContext().Get<TeachMeBackendContext>();
            }
        }

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

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public AuthController()
        {
            signingKey = Environment.GetEnvironmentVariable("WEBSITE_AUTH_SIGNING_KEY");

            if (string.IsNullOrEmpty(signingKey))
            {
                // WEBSITE_AUTH_SIGNING_KEY - is null, when it is run locally for debugging
                signingKey = ConfigurationManager.AppSettings["SigningKey"];
                audience = ConfigurationManager.AppSettings["ValidAudience"];
                issuer = ConfigurationManager.AppSettings["ValidIssuer"];
            }
            else
            {
                var website = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");
                audience = $"https://{website}/";
                issuer = $"https://{website}/";
            }


            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["JwtTokenExpirationTimeInHours"]))
            {
                jwtTokenExpirationTimeInHours = Int32.Parse(ConfigurationManager.AppSettings["JwtTokenExpirationTimeInHours"]);
            }
            else
            {
                jwtTokenExpirationTimeInHours = 72;
            }
            
        }
                
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await UserManager.FindByIdAsync(Id);

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
            var claimsPrincipal = this.User as ClaimsPrincipal;
            response.Sid = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
            //response.Sid = claimsPrincipal.FindFirst(ClaimTypes.GivenName).Value;
            response.Role = claimsPrincipal.FindFirst(ClaimTypes.Role).Value;           

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

                dbContext.Set<User>().Add(user);
                dbContext.SaveChanges();

                // Create token for the new registered user
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, model.Email),
                    new Claim(JwtRegisteredClaimNames.GivenName, appUser.FullName),
                    new Claim(ClaimTypes.Role, model.Role.ToString())
                };

                var token = AppServiceLoginHandler.CreateToken(claims, signingKey, audience, issuer, TimeSpan.FromDays(jwtTokenExpirationTimeInHours));

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
                    new Claim(JwtRegisteredClaimNames.GivenName, appUser.FullName)
                };

            var userRoles = await UserManager.GetRolesAsync(appUser.Id);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = AppServiceLoginHandler.CreateToken(claims, signingKey, audience, issuer, TimeSpan.FromHours(jwtTokenExpirationTimeInHours));

            var user = dbContext.Set<User>().Find(appUser.Id);

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
