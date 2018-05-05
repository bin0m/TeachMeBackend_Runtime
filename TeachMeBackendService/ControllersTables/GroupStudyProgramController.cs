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
    [RoutePrefix("tables/PartyStudyProgram")]
    public class PartyStudyProgramController : TableController<PartyStudyProgram>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<PartyStudyProgram>(context, Request);
        }

        // GET tables/PartyStudyProgram
        public IQueryable<PartyStudyProgram> GetAllPartyStudyProgram()
        {
            return Query(); 
        }

        // GET tables/PartyStudyProgram/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<PartyStudyProgram> GetPartyStudyProgram(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/PartyStudyProgram/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<PartyStudyProgram> PatchPartyStudyProgram(string id, Delta<PartyStudyProgram> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/PartyStudyProgram
        public async Task<IHttpActionResult> PostPartyStudyProgram(PartyStudyProgram item)
        {
            PartyStudyProgram current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/PartyStudyProgram/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePartyStudyProgram(string id)
        {
             return DeleteAsync(id);
        }
    }
}
