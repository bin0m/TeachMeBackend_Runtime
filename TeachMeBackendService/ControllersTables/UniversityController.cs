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
    [RoutePrefix("tables/University")]
    public class UniversityController : TableController<University>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<University>(context, Request);
        }

        // GET tables/University
        public IQueryable<University> GetAllUniversity()
        {
            return Query(); 
        }

        // GET tables/University/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<University> GetUniversity(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/University/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<University> PatchUniversity(string id, Delta<University> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/University
        public async Task<IHttpActionResult> PostUniversity(University item)
        {
            University current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/University/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteUniversity(string id)
        {
             return DeleteAsync(id);
        }
    }
}
