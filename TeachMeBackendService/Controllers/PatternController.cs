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
    public class PatternController : TableController<Pattern>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Pattern>(context, Request);
        }

        // GET tables/Pattern
        public IQueryable<Pattern> GetAllPattern()
        {
            return Query(); 
        }

        // GET tables/Pattern/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Pattern> GetPattern(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Pattern/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Pattern> PatchPattern(string id, Delta<Pattern> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Pattern
        public async Task<IHttpActionResult> PostPattern(Pattern item)
        {
            Pattern current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Pattern/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePattern(string id)
        {
             return DeleteAsync(id);
        }
    }
}
