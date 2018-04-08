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
    [RoutePrefix("tables/SectionProgress")]
    public class SectionProgressController : TableController<SectionProgress>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<SectionProgress>(context, Request);
        }

        // GET tables/SectionProgress
        [Route("")]
        public IQueryable<SectionProgress> GetAllSectionProgress()
        {
            return Query(); 
        }

        // GET tables/SectionProgress/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public SingleResult<SectionProgress> GetSectionProgress(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/SectionProgress/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<SectionProgress> PatchSectionProgress(string id, Delta<SectionProgress> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/SectionProgress
        [Route("")]
        public async Task<IHttpActionResult> PostSectionProgress(SectionProgress item)
        {
            SectionProgress current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/SectionProgress/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteSectionProgress(string id)
        {
             return DeleteAsync(id);
        }
    }
}
