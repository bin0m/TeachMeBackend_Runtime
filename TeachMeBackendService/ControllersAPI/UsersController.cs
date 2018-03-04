using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Web.Http;
using TeachMeBackendService.DataObjects;
using Microsoft.AspNet.Identity.Owin;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.ControllersAPI
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:ApiVersion}/users")]
    [MobileAppController]
    [Authorize]
    public class UsersController : ApiController
    {
        TeachMeBackendContext Db => Request.GetOwinContext().Get<TeachMeBackendContext>();


        // GET: api/Users
        [Route("")]
        public IQueryable<User> GetUsers()
        {
            return Db.UserDetails;
        }

        // GET: api/Users/5
        [Route("{id}")]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string id)
        {
            User user = Db.UserDetails.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}