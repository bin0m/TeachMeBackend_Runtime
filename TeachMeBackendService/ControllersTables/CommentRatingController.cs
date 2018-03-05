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
    [RoutePrefix("api/v{version:ApiVersion}/commentrating")]
    [Authorize]
    public class CommentRatingController : TableController<CommentRating>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<CommentRating>(context, Request);
        }

        // GET tables/CommentRating
        [Route("")]
        public IQueryable<CommentRating> GetAllCommentRating()
        {
            return Query(); 
        }

        // GET tables/CommentRating/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetCommentRating")]
        public SingleResult<CommentRating> GetCommentRating(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/CommentRating/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<CommentRating> PatchCommentRating(string id, Delta<CommentRating> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/CommentRating
        [Route("")]
        public async Task<IHttpActionResult> PostCommentRating(CommentRating item)
        {
            CommentRating current = await InsertAsync(item);
            return CreatedAtRoute("GetCommentRating", new { id = current.Id }, current);
        }

        // DELETE tables/CommentRating/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteCommentRating(string id)
        {
             return DeleteAsync(id);
        }
    }
}
