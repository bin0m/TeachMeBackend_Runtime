using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.Mobile.Server.Login;
using Microsoft.Web.Http;
using System;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
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


        // POST api/v1.0/auth/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email, FullName = "John Lewis"   };

            try
            {
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
           

            return Ok();
        }

        [AllowAnonymous]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(LoginViewModel model, string returnUrl = null)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var user = await UserManager.FindAsync(model.Email, model.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, model.Email)
            };

            var token = AppServiceLoginHandler.CreateToken(
                claims, signingKey, audience, issuer, TimeSpan.FromDays(30));


            return Ok(new LoginResult()
            {
                AuthenticationToken = token.RawData,
                User = new LoginResultUser { UserId = user.UserName }
            });
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
