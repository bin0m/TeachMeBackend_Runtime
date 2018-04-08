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
    [RoutePrefix("api/v{version:ApiVersion}/comment")]
    [Authorize]
    public class CommentController : TableController<Comment>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Comment>(context, Request);
        }

        // GET Comment
        [Route("")]
        public IQueryable<Comment> GetAllComment()
        {
            return Query(); 
        }

        // GET Comment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetComment")]
        public SingleResult<Comment> GetComment(string id)
        {
            return Lookup(id);
        }

        // PATCH Comment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<Comment> PatchComment(string id, Delta<Comment> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST Comment
        [Route("")]
        public async Task<IHttpActionResult> PostComment(Comment item)
        {
            Comment current = await InsertAsync(item);
            return CreatedAtRoute("GetComment", new { id = current.Id }, current);
        }

        // DELETE Comment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteComment(string id)
        {
             return DeleteAsync(id);
        }
    }
}
