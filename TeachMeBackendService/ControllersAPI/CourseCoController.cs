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
    [RoutePrefix("api/v{version:ApiVersion}/course")]
    public class CourseCoController : TableController<Course>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Course>(context, Request);
        }

        // GET tables/Course
        [Route("")]
        public IQueryable<Course> GetAllCourse()
        {
            return Query();
        }

        // GET tables/Course/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetCourseCo")]
        public SingleResult<Course> GetCourse(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Course/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<Course> PatchCourse(string id, Delta<Course> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Course
        [Route("")]
        public async Task<IHttpActionResult> PostCourse(Course item)
        {
            Course current = await InsertAsync(item);
            return CreatedAtRoute("GetCourseCo", new { id = current.Id }, current);
        }

        // DELETE tables/Course/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteCourse(string id)
        {
            return DeleteAsync(id);
        }
    }
}
