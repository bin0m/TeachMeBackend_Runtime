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
    [RoutePrefix("tables/Party")]
    public class PartyController : TableController<Party>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Party>(context, Request);
        }

        // GET tables/Party
        public IQueryable<Party> GetAllParty()
        {
            return Query(); 
        }

        // GET tables/Party/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Party> GetParty(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Party/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Party> PatchParty(string id, Delta<Party> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Party
        public async Task<IHttpActionResult> PostParty(Party item)
        {
            Party current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Party/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteParty(string id)
        {
             return DeleteAsync(id);
        }
    }
}
