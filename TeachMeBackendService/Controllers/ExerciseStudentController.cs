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
    [RoutePrefix("api/v{version:ApiVersion}/exercisestudent")]
    public class ExerciseStudentController : TableController<ExerciseStudent>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<ExerciseStudent>(context, Request);
        }

        // GET tables/ExerciseStudent
        [Route("")]
        public IQueryable<ExerciseStudent> GetAllExerciseStudent()
        {
            return Query(); 
        }

        // GET tables/ExerciseStudent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetExerciseStudent")]
        public SingleResult<ExerciseStudent> GetExerciseStudent(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/ExerciseStudent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<ExerciseStudent> PatchExerciseStudent(string id, Delta<ExerciseStudent> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/ExerciseStudent
        [Route("")]
        public async Task<IHttpActionResult> PostExerciseStudent(ExerciseStudent item)
        {
            ExerciseStudent current = await InsertAsync(item);
            return CreatedAtRoute("GetExerciseStudent", new { id = current.Id }, current);
        }

        // DELETE tables/ExerciseStudent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteExerciseStudent(string id)
        {
             return DeleteAsync(id);
        }
    }
}
