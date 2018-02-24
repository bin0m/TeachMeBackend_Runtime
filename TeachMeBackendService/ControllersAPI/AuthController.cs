using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.Mobile.Server.Login;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
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
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private string signingKey;
        private string audience;
        private string issuer;

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
            var website = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");
            audience = $"https://{website}/";
            issuer = $"https://{website}/";
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
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
            // Get the SID of the current user.
            var claimsPrincipal = this.User as ClaimsPrincipal;
            response.Sid = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
            //response.Role = claimsPrincipal.FindFirst(ClaimTypes.Role).Value;
            response.Role = "";

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

            var appUser = new ApplicationUser() { UserName = model.Email, Email = model.Email, FullName = model.FullName };
            User user;

            try
            {
                IdentityResult result = await UserManager.CreateAsync(appUser, model.Password);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }


                await UserManager.AddClaimAsync(appUser.Id, new Claim(ClaimTypes.Name, appUser.UserName));
                await UserManager.AddClaimAsync(appUser.Id, new Claim(ClaimTypes.Role, model.Role.ToString()));
                await UserManager.AddToRoleAsync(appUser.Id, model.Role.ToString());

                user = new User
                {
                    Id = appUser.Id,
                    Uid = appUser.Id,
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

            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return BadRequest(message);
            }
           

            return Ok(user);
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

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, model.Email),
                    new Claim(JwtRegisteredClaimNames.FamilyName, appUser.FullName)
                };

                var userRoles = await UserManager.GetRolesAsync(appUser.Id);
                foreach (var userRole in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = AppServiceLoginHandler.CreateToken( claims, signingKey, audience, issuer, TimeSpan.FromDays(30));

                var user = dbContext.Set<User>().Find(appUser.Id);

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

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

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
