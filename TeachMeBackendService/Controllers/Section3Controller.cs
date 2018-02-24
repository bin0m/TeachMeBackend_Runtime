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
    [RoutePrefix("api/v{version:ApiVersion}/section3")]
    [Authorize]
    public class Section3Controller : TableController<Section3>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Section3>(context, Request);
        }

        // GET tables/Section3
        [Route("")]
        public IQueryable<Section3> GetAllSection3()
        {
            return Query(); 
        }

        // GET tables/Section3/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetSection3")]
        public SingleResult<Section3> GetSection3(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Section3/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<Section3> PatchSection3(string id, Delta<Section3> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Section3
        [Route("")]
        public async Task<IHttpActionResult> PostSection3(Section3 item)
        {
            Section3 current = await InsertAsync(item);
            return CreatedAtRoute("GetSection3", new { id = current.Id }, current);
        }

        // DELETE tables/Section3/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteSection3(string id)
        {
             return DeleteAsync(id);
        }
    }
}
