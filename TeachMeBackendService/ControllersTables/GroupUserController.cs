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
    [RoutePrefix("tables/PartyUser")]
    public class PartyUserController : TableController<PartyUser>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<PartyUser>(context, Request);
        }

        // GET tables/PartyUser
        public IQueryable<PartyUser> GetAllPartyUser()
        {
            return Query(); 
        }

        // GET tables/PartyUser/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<PartyUser> GetPartyUser(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/PartyUser/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<PartyUser> PatchPartyUser(string id, Delta<PartyUser> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/PartyUser
        public async Task<IHttpActionResult> PostPartyUser(PartyUser item)
        {
            PartyUser current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/PartyUser/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePartyUser(string id)
        {
             return DeleteAsync(id);
        }
    }
}
