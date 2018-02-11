using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Web.Http;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:ApiVersion}/studentcourse")]
    public class StudentCourseController : TableController<StudentCourse>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<StudentCourse>(context, Request);
        }

        // GET tables/StudentCourse
        [Route("")]
        public IQueryable<StudentCourse> GetAllStudentCourse()
        {
            return Query(); 
        }

        // GET tables/StudentCourse/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetStudentCourse")]
        public SingleResult<StudentCourse> GetStudentCourse(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/StudentCourse/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<StudentCourse> PatchStudentCourse(string id, Delta<StudentCourse> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/StudentCourse
        [Route("")]
        public async Task<IHttpActionResult> PostStudentCourse(StudentCourse item)
        {
            StudentCourse current = await InsertAsync(item);
            return CreatedAtRoute("GetStudentCourse", new { id = current.Id }, current);
        }

        // DELETE tables/StudentCourse/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteStudentCourse(string id)
        {
             return DeleteAsync(id);
        }
    }
}
