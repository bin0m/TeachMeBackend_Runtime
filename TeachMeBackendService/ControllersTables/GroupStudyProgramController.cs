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
    [RoutePrefix("tables/GroupStudyProgram")]
    public class GroupStudyProgramController : TableController<GroupStudyProgram>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<GroupStudyProgram>(context, Request);
        }

        // GET tables/GroupStudyProgram
        public IQueryable<GroupStudyProgram> GetAllGroupStudyProgram()
        {
            return Query(); 
        }

        // GET tables/GroupStudyProgram/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<GroupStudyProgram> GetGroupStudyProgram(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/GroupStudyProgram/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<GroupStudyProgram> PatchGroupStudyProgram(string id, Delta<GroupStudyProgram> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/GroupStudyProgram
        public async Task<IHttpActionResult> PostGroupStudyProgram(GroupStudyProgram item)
        {
            GroupStudyProgram current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/GroupStudyProgram/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteGroupStudyProgram(string id)
        {
             return DeleteAsync(id);
        }
    }
}
