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
        private readonly TeachMeBackendContext _db = new TeachMeBackendContext();

        // GET: api/Lessons
        [Route("")]
        public IEnumerable<Lesson> GetLessons()
        {
            var all = _db.Lessons.OrderBy(x => x.CreatedAt).ToList();
            all.ForEach(x => x.Progress = CalculateLessonProgress(x.Id));
            return all;
        }

        // GET: api/Lessons/5
        [Route("{id}")]
        [ResponseType(typeof(Lesson))]
        public IHttpActionResult GetLesson(string id)
        {
            Lesson lesson = _db.Lessons.Find(id);
            if (lesson == null)
            {
                return NotFound();
            }

            //Calculates progress for all exercises under this lesson for the current user
            lesson.Progress = CalculateLessonProgress(id);

            return Ok(lesson);
        }

        // GET: api/Lessons/5/progress
        [Route("{id}/progress")]
        [ResponseType(typeof(ProgressLessonModel))]
        public IHttpActionResult GetLessonProgress(string id)
        {
            Lesson lesson = _db.Lessons.Find(id);
            if (lesson == null)
            {
                return NotFound();
            }

            //Calculates progress for all exercises under this lesson for the current user
            ProgressLessonModel progress = CalculateLessonProgress(id);

            return Ok(progress);
        }

        //Calculates progress for all exercises under this lesson for the current user
        private ProgressLessonModel CalculateLessonProgress(string id)
        {
            ProgressLessonModel progressLessonModel = new ProgressLessonModel();
            var exercises = _db.Exercises.Where(ex => ex.LessonId == id).Include(ex => ex.ExerciseStudents);
            progressLessonModel.ExercisesNumber = exercises.Count();
            if (User is ClaimsPrincipal claimsPrincipal)
            {
                var userId = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid).Value;
                    progressLessonModel.ExercisesDone =
                    exercises.Count(ex => ex.ExerciseStudents.Any(c => c.UserId == userId && c.IsDone));
                var lessonProgress = _db.LessonProgresses.FirstOrDefault(p => p.UserId == userId && p.LessonId == id);
                if (lessonProgress != null)
                {
                    progressLessonModel.IsDone = lessonProgress.IsDone;
                    progressLessonModel.IsStarted = lessonProgress.IsDone;
                }
            }
           
            return progressLessonModel;
        }


        [Route("~/api/v{version:ApiVersion}/sections/{id}/lessons")]
        public IQueryable<Lesson> GetBySection(string id)
        {
            var lessons = _db.Lessons.Where(c => c.SectionId == id);

            return lessons;
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