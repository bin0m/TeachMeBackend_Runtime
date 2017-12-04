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
    public class StudentController : TableController<Student>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Student>(context, Request);
        }

        // GET tables/Student
        public IQueryable<Student> GetAllStudent()
        {
            var query = Query();
            if (query.Count<Student>() == 0)
            {
                List<Student> testStudentsSet = new List<Student> {
                new Student {
                    Id = Guid.NewGuid().ToString(),
                    CompletedСoursesCount = 0,
                    Email = "nikolaev12@mail.ru",
                    FullName = "Сергей Николаев",
                    Login = "nikolaev",
                    Password = "aFA4baf34B9e83b3876e=",
                    RegisterDate = new DateTime(2017, 12, 1),
                    },
                new Student {
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
                    foreach (Student Student in testStudentsSet)
                    {
                        context.Set<Student>().Add(Student);
                    }
                    context.SaveChanges();
                }

            }
            return query;
        }

        // GET tables/Student/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Student> GetStudent(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Student/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Student> PatchStudent(string id, Delta<Student> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Student
        public async Task<IHttpActionResult> PostStudent(Student item)
        {
            Student current = new Student();
            try
            {
                //item.RegisterDate = System.DateTime.Now;
                current = await InsertAsync(item);
            }
            catch (Exception ex)
            {
                Configuration.Services.GetTraceWriter().Error(ex, category: "PostStudent");
            }
            
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Student/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteStudent(string id)
        {
             return DeleteAsync(id);
        }
    }
}
