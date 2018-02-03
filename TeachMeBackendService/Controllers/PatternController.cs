using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Web.Http;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:ApiVersion}/pattern")]
    public class PatternController : TableController<Pattern>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Pattern>(context, Request);
        }

        // GET tables/Pattern
        [Route("")]
        public IQueryable<Pattern> GetAllPattern()
        {
            return Query(); 
        }

        // GET tables/Pattern/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetPattern")]
        public SingleResult<Pattern> GetPattern(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Pattern/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<Pattern> PatchPattern(string id, Delta<Pattern> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Pattern
        [Route("")]
        public async Task<IHttpActionResult> PostPattern(Pattern item)
        {
            Pattern current = await InsertAsync(item);
            return CreatedAtRoute("GetPattern", new { id = current.Id }, current);
        }

        // DELETE tables/Pattern/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeletePattern(string id)
        {
             return DeleteAsync(id);
        }
    }
}
