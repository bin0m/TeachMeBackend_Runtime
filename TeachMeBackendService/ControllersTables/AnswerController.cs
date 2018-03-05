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
    [RoutePrefix("api/v{version:ApiVersion}/answer")]
    [Authorize]
    public class AnswerController : TableController<Answer>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Answer>(context, Request);
        }

        // GET tables/Answer
        [Route("")]
        public IQueryable<Answer> GetAllAnswer()
        {
            return Query(); 
        }

        // GET tables/Answer/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetAnswer")]
        public SingleResult<Answer> GetAnswer(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Answer/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<Answer> PatchAnswer(string id, Delta<Answer> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Answer
        [Route("")]
        public async Task<IHttpActionResult> PostAnswer(Answer item)
        {
            Answer current = await InsertAsync(item);
            return CreatedAtRoute("GetAnswer", new { id = current.Id }, current);
        }

        // DELETE tables/Answer/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteAnswer(string id)
        {
             return DeleteAsync(id);
        }
    }
}
