using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Web.Http;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.ControllersAPI
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:ApiVersion}/courses")]
    [MobileAppController]
    [Authorize]
    public class CoursesController : ApiController
    {
        private readonly TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/Courses
        [Route("")]
        public IEnumerable<Course> GetCourses()
        {
            var all = db.Courses.OrderBy(x => x.CreatedAt).ToList();
            all.ForEach(x => x.Progress = CalculateCourseProgress(x.Id));
            return all;
        }

        // GET: api/Courses/5
        [Route("{id}")]
        [ResponseType(typeof(Course))]
        public IHttpActionResult GetCourse(string id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            //Calculates progress for all sections under this course for the current user
            course.Progress = CalculateCourseProgress(id);

            return Ok(course);
        }

        // GET: api/Courses/5/progress
        [Route("{id}/progress")]
        [ResponseType(typeof(ProgressCourseModel))]
        public IHttpActionResult GetCourseProgress(string id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            //Calculates progress for all exercises under this course for the current user
            ProgressCourseModel progress = CalculateCourseProgress(id);

            return Ok(progress);
        }

        //Calculates progress for all sections under this course for the current user
        private ProgressCourseModel CalculateCourseProgress(string id)
        {
            ProgressCourseModel progressCourseModel = new ProgressCourseModel();
            var sections = db.Sections.Where(c => c.CourseId == id).Include(c => c.SectionProgresses);
            progressCourseModel.SectionsNumber = sections.Count();
            if (User is ClaimsPrincipal claimsPrincipal)
            {
                var userId = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid).Value;
                progressCourseModel.SectionsDone =
                    sections.Count(c => c.SectionProgresses.Any(p => p.UserId == userId && p.IsDone));
            }
            return progressCourseModel;
        }

        // DELETE: api/Courses/5
        [Route("{id}")]
        [ResponseType(typeof(Course))]
        public IHttpActionResult DeleteCourse(string id)
        {
            using (var dbContext = new TeachMeBackendContext())
            {
                var course = dbContext.DeleteCourseAndChildren(id);
                if (course == null)
                {
                    return NotFound();
                }
                dbContext.SaveChanges();
            }

            return StatusCode(HttpStatusCode.NoContent); 
        }

        [Route("~/api/v{version:ApiVersion}/users/{usesrId}/courses")]
        public IQueryable<Course> GetByUserId(string usesrId, bool isTeacher=false)
        {
            if (isTeacher)
            {
                var courses = db.Courses.Where(c => c.UserId == usesrId);
                return courses;
            }

            var query = from course in db.Courses
                        where course.CourseProgresses.Any(c => c.UserId == usesrId && c.IsStarted)
                        select course;

            return query;

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