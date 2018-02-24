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
using Microsoft.Web.Http;

namespace TeachMeBackendService.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:ApiVersion}/user")]
    [Authorize]
    public class UserController : TableController<User>
    {
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
            if (query.Count<User>() == 0)
            {
                List<User> testUsersSet = new List<User> {
                new User {
                    Id = Guid.NewGuid().ToString(),
                    CompletedCoursesCount = 0,
                    Email = "nikolaev12@mail.ru",
                    FullName = "Сергей Николаев",
                    Login = "nikolaev",
                    RegisterDate = new DateTime(2017, 12, 1),
                    },
                new User {
                    Id = Guid.NewGuid().ToString(),
                    CompletedCoursesCount = 0,
                    Email = "pugaeva.verchik@yandex.ru",
                    FullName = "Вероника Пугаева",
                    Login = "verchik",
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
             return UpdateAsync(id, patch);
        }

        // POST User
        [Route("")]
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

            return CreatedAtRoute("GetUser", new { id = current.Id }, current);
            //catch (HttpResponseException ex)
            //{
            //  string message = ((HttpError)((ObjectContent)ex.Response.Content).Value).First().Value.ToString();
            //      string[] temp = message.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            //      var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
            //      {
            //          Content = new StringContent(message),
            //          ReasonPhrase = temp[0]
            //      };
            //  throw new HttpResponseException(resp);
            //  }

        }


        // DELETE User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteUser(string id)
        {
             return DeleteAsync(id);
        }
    }
}
