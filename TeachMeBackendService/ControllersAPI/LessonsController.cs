using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
    [RoutePrefix("api/v{version:ApiVersion}/lessons")]
    [MobileAppController]
    [Authorize]
    public class LessonsController : ApiController
    {
        private TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/Lessons
        [Route("")]
        public IQueryable<Lesson> GetLessons()
        {
            return db.Lessons;
        }

        // GET: api/Lessons/5
        [Route("{id}")]
        [ResponseType(typeof(Lesson))]
        public IHttpActionResult GetLesson(string id)
        {
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return NotFound();
            }

            var claimsPrincipal = User as ClaimsPrincipal;
            if (claimsPrincipal != null)
            {
                var userId = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid).Value;                
                var exercises = db.Exercises.Where(ex => ex.LessonId == id).Include(ex => ex.ExerciseStudents);
                lesson.ExercisesNumber = exercises.Count();

                //TODO: simplify loop ( use one LINQ query instead)
                foreach (var ex in exercises)
                {
                    var exerciseStudents = ex.ExerciseStudents.Where(c => c.UserId == userId).FirstOrDefault();
                    if (exerciseStudents != null)
                    {
                        if (exerciseStudents.IsDone)
                        {
                            lesson.ExercisesDone += 1;
                        }
                    }
                }
            }

            return Ok(lesson);
        }

        
        [Route("~/api/v{version:ApiVersion}/sections/{id}/lessons")]
        public IQueryable<Lesson> GetBySection(string id)
        {
            var lessons = db.Lessons.Where(c => c.SectionId == id);

            return lessons;
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