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
    [RoutePrefix("tables/LessonProgress")]
    public class LessonProgressController : TableController<LessonProgress>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<LessonProgress>(context, Request);
        }

        // GET tables/LessonProgress
        [Route("")]
        public IQueryable<LessonProgress> GetAllLessonProgress()
        {
            return Query(); 
        }

        // GET tables/LessonProgress/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public SingleResult<LessonProgress> GetLessonProgress(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/LessonProgress/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<LessonProgress> PatchLessonProgress(string id, Delta<LessonProgress> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/LessonProgress
        [Route("")]
        public async Task<IHttpActionResult> PostLessonProgress(LessonProgress item)
        {
            LessonProgress current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/LessonProgress/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteLessonProgress(string id)
        {
             return DeleteAsync(id);
        }
    }
}
