﻿using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Web.Http;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.ControllersTables
{
    [Authorize]
    [ApiVersionNeutral]
    [RoutePrefix("tables/User")]
    public class UserController : TableController<User>
    {
        TeachMeBackendContext Db => Request.GetOwinContext().Get<TeachMeBackendContext>();

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<User>(context, Request);
        }

        // GET tables/User
        [Route("")]
        public IQueryable<User> GetAllUser()
        {
            var query = Query();           
            return query;
        }

        // GET tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetUser")]
        public SingleResult<User> GetUser(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
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


        // DELETE tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteUser(string id)
        {
             return DeleteAsync(id);
        }
    }
}
