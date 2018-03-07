using Microsoft.AspNet.Identity.Owin;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.Mobile.Server.Login;
using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.ControllersAPI
{
    [ApiVersionNeutral]
    [Route(".auth/login/custom")]
    [MobileAppController]
    public class AuthCustomController : ApiController
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

        public AuthCustomController()
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
             

        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]LoginViewModel model)
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
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
        
    }
}
