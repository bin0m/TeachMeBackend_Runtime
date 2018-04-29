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
    [RoutePrefix("tables/StudyProgramCourse")]
    public class StudyProgramCourseController : TableController<StudyProgramCourse>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<StudyProgramCourse>(context, Request);
        }

        // GET tables/StudyProgramCourse
        public IQueryable<StudyProgramCourse> GetAllStudyProgramCourse()
        {
            return Query(); 
        }

        // GET tables/StudyProgramCourse/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<StudyProgramCourse> GetStudyProgramCourse(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/StudyProgramCourse/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<StudyProgramCourse> PatchStudyProgramCourse(string id, Delta<StudyProgramCourse> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/StudyProgramCourse
        public async Task<IHttpActionResult> PostStudyProgramCourse(StudyProgramCourse item)
        {
            StudyProgramCourse current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/StudyProgramCourse/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteStudyProgramCourse(string id)
        {
             return DeleteAsync(id);
        }
    }
}
