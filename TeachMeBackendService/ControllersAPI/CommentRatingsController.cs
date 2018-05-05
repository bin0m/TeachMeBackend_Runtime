using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Web.Http;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.ControllersAPI
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:ApiVersion}/commentratings")]
    [MobileAppController]
    [Authorize]
    public class CommentRatingsController : ApiController
    {
        private readonly TeachMeBackendContext _db = new TeachMeBackendContext();

        // GET: api/CommentRatings
        [Route("")]
        public IQueryable<CommentRating> GetCommentRatings()
        {
            return _db.CommentRatings;
        }

        // GET: api/CommentRatings/5
        [Route("{id}")]
        [ResponseType(typeof(CommentRating))]
        public IHttpActionResult GetCommentRating(string id)
        {
            CommentRating commentRating = _db.CommentRatings.Find(id);
            if (commentRating == null)
            {
                return NotFound();
            }

            return Ok(commentRating);
        }


        [Route("~/api/v{version:ApiVersion}/comments/{id}/commentratings")]
        public IQueryable<CommentRating> GetBySection(string id)
        {
            var commentRating = _db.CommentRatings.Where(c => c.CommentId == id);

            return commentRating;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}