using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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
        private readonly TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/Lessons
        [Route("")]
        public IEnumerable<Lesson> GetLessons()
        {
            var all = db.Lessons.OrderBy(x => x.CreatedAt).ToList();
            all.ForEach(x => x.Progress = CalculateLessonProgress(x.Id));
            return all;
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

            //Calculates progress for all exercises under this lesson for the current user
            lesson.Progress = CalculateLessonProgress(id);

            return Ok(lesson);
        }

        //Calculates progress for all exercises under this lesson for the current user
        private ProgressLessonModel CalculateLessonProgress(string id)
        {
            ProgressLessonModel progressLessonModel = new ProgressLessonModel();
            var exercises = db.Exercises.Where(ex => ex.LessonId == id).Include(ex => ex.ExerciseStudents);
            progressLessonModel.ExercisesNumber = exercises.Count();
            if (User is ClaimsPrincipal claimsPrincipal)
            {
                var userId = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid).Value;
                    progressLessonModel.ExercisesDone =
                    exercises.Count(ex => ex.ExerciseStudents.Any(c => c.UserId == userId && c.IsDone));
                var sectionProgress = db.LessonProgresses.FirstOrDefault(p => p.UserId == userId && p.LessonId == id);
                if (sectionProgress != null)
                {
                    progressLessonModel.IsDone = sectionProgress.IsDone;
                    progressLessonModel.IsStarted = sectionProgress.IsDone;
                }
            }
           
            return progressLessonModel;
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