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
    [RoutePrefix("api/v{version:ApiVersion}/patternstudent")]
    public class PatternStudentController : TableController<PatternStudent>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<PatternStudent>(context, Request);
        }

        // GET tables/PatternStudent
        [Route("")]
        public IQueryable<PatternStudent> GetAllPatternStudent()
        {
            return Query(); 
        }

        // GET tables/PatternStudent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetPatternStudent")]
        public SingleResult<PatternStudent> GetPatternStudent(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/PatternStudent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<PatternStudent> PatchPatternStudent(string id, Delta<PatternStudent> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/PatternStudent
        [Route("")]
        public async Task<IHttpActionResult> PostPatternStudent(PatternStudent item)
        {
            PatternStudent current = await InsertAsync(item);
            return CreatedAtRoute("GetPatternStudent", new { id = current.Id }, current);
        }

        // DELETE tables/PatternStudent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeletePatternStudent(string id)
        {
             return DeleteAsync(id);
        }
    }
}
