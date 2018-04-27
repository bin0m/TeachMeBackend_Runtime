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
    [RoutePrefix("tables/Faculty")]
    public class FacultyController : TableController<Faculty>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Faculty>(context, Request);
        }

        // GET tables/Faculty
        public IQueryable<Faculty> GetAllFaculty()
        {
            return Query(); 
        }

        // GET tables/Faculty/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Faculty> GetFaculty(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Faculty/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Faculty> PatchFaculty(string id, Delta<Faculty> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Faculty
        public async Task<IHttpActionResult> PostFaculty(Faculty item)
        {
            Faculty current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Faculty/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteFaculty(string id)
        {
             return DeleteAsync(id);
        }
    }
}
