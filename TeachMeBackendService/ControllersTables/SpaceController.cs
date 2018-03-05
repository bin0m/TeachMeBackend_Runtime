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
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:ApiVersion}/space")]
    [Authorize]
    public class SpaceController : TableController<Space>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Space>(context, Request);
        }

        // GET tables/Space
        [Route("")]
        public IQueryable<Space> GetAllSpace()
        {
            return Query(); 
        }

        // GET tables/Space/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetSpace")]
        public SingleResult<Space> GetSpace(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Space/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<Space> PatchSpace(string id, Delta<Space> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Space
        [Route("")]
        public async Task<IHttpActionResult> PostSpace(Space item)
        {
            Space current = await InsertAsync(item);
            return CreatedAtRoute("GetSpace", new { id = current.Id }, current);
        }

        // DELETE tables/Space/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteSpace(string id)
        {
             return DeleteAsync(id);
        }
    }
}
