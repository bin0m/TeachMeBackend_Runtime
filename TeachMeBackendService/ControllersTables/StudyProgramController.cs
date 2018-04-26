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
    [RoutePrefix("tables/StudyProgram")]
    public class StudyProgramController : TableController<StudyProgram>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<StudyProgram>(context, Request);
        }

        // GET tables/StudyProgram
        public IQueryable<StudyProgram> GetAllStudyProgram()
        {
            return Query(); 
        }

        // GET tables/StudyProgram/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<StudyProgram> GetStudyProgram(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/StudyProgram/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<StudyProgram> PatchStudyProgram(string id, Delta<StudyProgram> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/StudyProgram
        public async Task<IHttpActionResult> PostStudyProgram(StudyProgram item)
        {
            StudyProgram current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/StudyProgram/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteStudyProgram(string id)
        {
             return DeleteAsync(id);
        }
    }
}
