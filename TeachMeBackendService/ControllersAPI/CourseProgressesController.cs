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
    [RoutePrefix("api/v{version:ApiVersion}/CourseProgresses")]
    [MobileAppController]
    [Authorize]
    public class CourseProgresssController : ApiController
    {
        private TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/CourseProgresss
        [Route("")]
        public IQueryable<CourseProgress> GetCourseProgresss()
        {
            return db.CourseProgresses;
        }

        // GET: api/CourseProgresss/5
        [Route("{id}")]
        [ResponseType(typeof(CourseProgress))]
        public IHttpActionResult GetCourseProgress(string id)
        {
            CourseProgress CourseProgress = db.CourseProgresses.Find(id);
            if (CourseProgress == null)
            {
                return NotFound();
            }

            return Ok(CourseProgress);
        }

        
        [Route("~/api/v{version:ApiVersion}/users/{id}/CourseProgresss")]
        public IQueryable<CourseProgress> GetByExercise(string id)
        {
            var CourseProgresss = db.CourseProgresses.Where(c => c.UserId == id);

            return CourseProgresss;
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