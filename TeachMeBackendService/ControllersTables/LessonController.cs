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
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:ApiVersion}/lesson")]
    [Authorize]
    public class LessonController : TableController<Lesson>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Lesson>(context, Request);
        }

        // GET tables/Lesson
        [Route("")]
        public IQueryable<Lesson> GetAllLesson()
        {
            return Query(); 
        }

        // GET tables/Lesson/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetLesson")]
        public SingleResult<Lesson> GetLesson(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Lesson/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<Lesson> PatchLesson(string id, Delta<Lesson> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Lesson
        [Route("")]
        public async Task<IHttpActionResult> PostLesson(Lesson item)
        {
            Lesson current = await InsertAsync(item);
            return CreatedAtRoute("GetLesson", new { id = current.Id }, current);
        }

        // DELETE tables/Lesson/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteLesson(string id)
        {
             return DeleteAsync(id);
        }
    }
}
