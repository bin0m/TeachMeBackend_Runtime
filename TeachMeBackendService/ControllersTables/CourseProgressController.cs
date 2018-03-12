using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Web.Http;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.ControllersTables
{
    [Authorize]
    [ApiVersionNeutral]
    [RoutePrefix("tables/CourseProgress")]
    public class CourseProgressController : TableController<CourseProgress>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<CourseProgress>(context, Request);
        }

        // GET tables/CourseProgress
        [Route("")]
        public IQueryable<CourseProgress> GetAllCourseProgress()
        {
            return Query(); 
        }

        // GET tables/CourseProgress/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public SingleResult<CourseProgress> GetCourseProgress(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/CourseProgress/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<CourseProgress> PatchCourseProgress(string id, Delta<CourseProgress> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/CourseProgress
        [Route("")]
        public async Task<IHttpActionResult> PostCourseProgress(CourseProgress item)
        {
            CourseProgress current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/CourseProgress/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteCourseProgress(string id)
        {
             return DeleteAsync(id);
        }
    }
}
