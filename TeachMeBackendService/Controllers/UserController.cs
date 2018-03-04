using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;
using Microsoft.Web.Http;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;

namespace TeachMeBackendService.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:ApiVersion}/user")]
    [Authorize]
    public class UserController : TableController<User>
    {
        TeachMeBackendContext Db => Request.GetOwinContext().Get<TeachMeBackendContext>();

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<User>(context, Request);
        }

        // GET User
        [Route("")]
        public IQueryable<User> GetAllUser()
        {
            var query = Query();           
            return query;
        }

        // GET User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetUser")]
        public SingleResult<User> GetUser(string id)
        {
            return Lookup(id);
        }

        // PATCH User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<User> PatchUser(string id, Delta<User> patch)
        {
            // Update AppUser
            // ReSharper disable once LocalNameCapturedOnly
            User user;            
            var propertyNames = patch.GetChangedPropertyNames();
            var enumerable = propertyNames as string[] ?? propertyNames.ToArray();
            bool isEmailChanged = enumerable.Contains(nameof(user.Email));
            bool isFullNameChanged = enumerable.Contains(nameof(user.FullName));

            if (isEmailChanged || isFullNameChanged)
            {
                var appUser = Db.Users.Find(id);
                if (isEmailChanged)
                {
                    patch.TryGetPropertyValue(nameof(user.Email), out var email);
                    appUser.Email = (string)email;
                    appUser.UserName = (string)email;
                }
                if (isFullNameChanged)
                {
                    patch.TryGetPropertyValue(nameof(user.FullName), out var fullName);
                    appUser.FullName = (string)fullName;
                }
                Db.SaveChanges();
            }

            // Update User
            return UpdateAsync(id, patch);
        }       


        // DELETE User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteUser(string id)
        {
             return DeleteAsync(id);
        }
    }
}
