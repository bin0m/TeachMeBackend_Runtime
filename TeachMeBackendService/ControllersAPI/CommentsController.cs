using System.Data.Entity;
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
    [RoutePrefix("api/v{version:ApiVersion}/comments")]
    [MobileAppController]
    [Authorize]
    public class CommentsController : ApiController
    {
        private TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/Comments
        [Route("")]
        public IQueryable<Comment> GetComments()
        {
            return db.Comments;
        }

        // GET: api/Comments/5
        [Route("{id}")]
        [ResponseType(typeof(Comment))]
        public IHttpActionResult GetComment(string id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }


        [Route("~/api/v{version:ApiVersion}/exercises/{id}/comments")]
        public IQueryable<Comment> GetBySection(string id)
        {
            //todo: rework this attempt to include User
            var comment = db.Comments.Include(c => c.User).Where(c => c.ExerciseId == id);

            return comment;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}