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
    [RoutePrefix("api/v{version:ApiVersion}/exercise")]
    [Authorize]
    public class ExerciseController : TableController<Exercise>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Exercise>(context, Request);
        }

        // GET tables/Exercise
        [Route("")]
        public IQueryable<Exercise> GetAllExercise()
        {
            return Query(); 
        }

        // GET tables/Exercise/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetExercise")]
        public SingleResult<Exercise> GetExercise(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Exercise/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<Exercise> PatchExercise(string id, Delta<Exercise> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Exercise
        [Route("")]
        public async Task<IHttpActionResult> PostExercise(Exercise item)
        {
            Exercise current = await InsertAsync(item);
            return CreatedAtRoute("GetExercise", new { id = current.Id }, current);
        }

        // DELETE tables/Exercise/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteExercise(string id)
        {
             return DeleteAsync(id);
        }
    }
}
