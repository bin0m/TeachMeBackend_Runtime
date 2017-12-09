using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;
using System.Collections.Generic;
using System;

namespace TeachMeBackendService.Controllers
{
    public class UserController : TableController<User>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<User>(context, Request);
        }

        // GET tables/User
        public IQueryable<User> GetAllUser()
        {
            var query = Query();
            if (query.Count<User>() == 0)
            {
                List<User> testUsersSet = new List<User> {
                new User {
                    Id = Guid.NewGuid().ToString(),
                    CompletedСoursesCount = 0,
                    Email = "nikolaev12@mail.ru",
                    FullName = "Сергей Николаев",
                    Login = "nikolaev",
                    Password = "ThisisMyPassw0rd",
                    RegisterDate = new DateTime(2017, 12, 1),
                    },
                new User {
                    Id = Guid.NewGuid().ToString(),
                    CompletedСoursesCount = 0,
                    Email = "pugaeva.verchik@yandex.ru",
                    FullName = "Вероника Пугаева",
                    Login = "verchik",
                    Password = "4C0brainik6EF0B",
                    RegisterDate = new DateTime(2017, 12, 2),
                    }
                };

                using (TeachMeBackendContext context = new TeachMeBackendContext())
                {
                    foreach (User User in testUsersSet)
                    {
                        context.Set<User>().Add(User);
                    }
                    context.SaveChanges();
                }

            }
            return query;
        }

        // GET tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<User> GetUser(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<User> PatchUser(string id, Delta<User> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/User
        public async Task<IHttpActionResult> PostUser(User item)
        {
            User current = new User();

            // Password hashihg with salt
            //var keyNew = Logic.Helper.GenerateSalt(10);
            //var password = Logic.Helper.EncodePassword(item.Password, keyNew);
            //item.Password = password;
            //item.Salt = keyNew;

             try
            {
                //item.RegisterDate = System.DateTime.Now;
                current = await InsertAsync(item);
            }
            catch (Exception ex)
            {
                Configuration.Services.GetTraceWriter().Error(ex, category: "PostUser");
                throw ex;
            }
            
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteUser(string id)
        {
             return DeleteAsync(id);
        }
    }
}
