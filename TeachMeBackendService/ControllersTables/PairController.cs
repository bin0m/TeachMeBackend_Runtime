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
    [RoutePrefix("tables/pair")]
    public class PairController : TableController<Pair>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Pair>(context, Request);
        }

        // GET tables/Pair
        [Route("")]
        public IQueryable<Pair> GetAllPair()
        {
            return Query(); 
        }

        // GET tables/Pair/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetPair")]
        public SingleResult<Pair> GetPair(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Pair/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<Pair> PatchPair(string id, Delta<Pair> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Pair
        [Route("")]
        public async Task<IHttpActionResult> PostPair(Pair item)
        {
            Pair current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Pair/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeletePair(string id)
        {
             return DeleteAsync(id);
        }
    }
}
