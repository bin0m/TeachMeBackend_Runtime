using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.Controllers
{
    public class SpaceController : TableController<Space>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Space>(context, Request);
        }

        // GET tables/Space
        public IQueryable<Space> GetAllSpace()
        {
            return Query(); 
        }

        // GET tables/Space/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Space> GetSpace(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Space/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Space> PatchSpace(string id, Delta<Space> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Space
        public async Task<IHttpActionResult> PostSpace(Space item)
        {
            Space current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Space/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteSpace(string id)
        {
             return DeleteAsync(id);
        }
    }
}
