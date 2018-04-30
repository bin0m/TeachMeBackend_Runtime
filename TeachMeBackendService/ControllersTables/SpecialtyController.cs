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
    [RoutePrefix("tables/Specialty")]
    public class SpecialtyController : TableController<Specialty>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Specialty>(context, Request);
        }

        // GET tables/Specialty
        public IQueryable<Specialty> GetAllSpecialty()
        {
            return Query(); 
        }

        // GET tables/Specialty/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Specialty> GetSpecialty(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Specialty/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Specialty> PatchSpecialty(string id, Delta<Specialty> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Specialty
        public async Task<IHttpActionResult> PostSpecialty(Specialty item)
        {
            Specialty current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Specialty/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteSpecialty(string id)
        {
             return DeleteAsync(id);
        }
    }
}
