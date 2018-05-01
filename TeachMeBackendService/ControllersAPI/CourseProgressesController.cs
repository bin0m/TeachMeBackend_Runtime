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
        private readonly TeachMeBackendContext _db = new TeachMeBackendContext();

        // GET: api/CourseProgresss
        [Route("")]
        public IQueryable<CourseProgress> GetCourseProgresss()
        {
            return _db.CourseProgresses;
        }

        // GET: api/CourseProgresss/5
        [Route("{id}")]
        [ResponseType(typeof(CourseProgress))]
        public IHttpActionResult GetCourseProgress(string id)
        {
            CourseProgress courseProgress = _db.CourseProgresses.Find(id);
            if (courseProgress == null)
            {
                return NotFound();
            }

            return Ok(courseProgress);
        }

        
        [Route("~/api/v{version:ApiVersion}/users/{id}/CourseProgresss")]
        public IQueryable<CourseProgress> GetByExercise(string id)
        {
            var courseProgresss = _db.CourseProgresses.Where(c => c.UserId == id);

            return courseProgresss;
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