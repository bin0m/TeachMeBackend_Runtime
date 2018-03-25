using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Web.Http;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.ControllersTables
{
    [Authorize]
    [ApiVersionNeutral]
    [RoutePrefix("tables/lesson")]
    public class LessonController : TableController<Lesson>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Lesson>(context, Request);
        }

        // GET tables/Lesson
        [Route("")]
        public IQueryable<Lesson> GetAllLesson()
        {
            return Query(); 
        }

        // GET tables/Lesson/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetLesson")]
        public SingleResult<Lesson> GetLesson(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Lesson/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<Lesson> PatchLesson(string id, Delta<Lesson> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Lesson
        [Route("")]
        public async Task<IHttpActionResult> PostLesson(Lesson item)
        {
            Lesson current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Lesson/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteLesson(string id)
        {
             return DeleteAsync(id);
        }

        // GET tables/Lesson/48D68C86-6EA6-4C25-AA33-223FC9A27959/progress
        [Route("{id}/progress")]
        [ResponseType(typeof(ProgressLessonModel))]
        public IHttpActionResult GetLessonProgress(string id)
        {
            using (var db = new TeachMeBackendContext())
            {
                Lesson lesson = db.Lessons.Find(id);
                if (lesson == null)
                {
                    return NotFound();
                }
            }

            //Calculates progress for all exercises under this lesson for the current user
            ProgressLessonModel progress = CalculateLessonProgress(id);

            return Ok(progress);
        }

        //Calculates progress for all exercises under this lesson for the current user
        private ProgressLessonModel CalculateLessonProgress(string id)
        {
            ProgressLessonModel progressLessonModel = new ProgressLessonModel();

            using (var db = new TeachMeBackendContext())
            {
                var exercises = db.Exercises.Where(ex => ex.LessonId == id).Include(ex => ex.ExerciseStudents);
                progressLessonModel.ExercisesNumber = exercises.Count();
                if (User is ClaimsPrincipal claimsPrincipal)
                {
                    var userId = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid).Value;
                    progressLessonModel.ExercisesDone =
                        exercises.Count(ex => ex.ExerciseStudents.Any(c => c.UserId == userId && c.IsDone));
                    var lessonProgress =
                        db.LessonProgresses.FirstOrDefault(p => p.UserId == userId && p.LessonId == id);
                    if (lessonProgress != null)
                    {
                        progressLessonModel.IsDone = lessonProgress.IsDone;
                        progressLessonModel.IsStarted = lessonProgress.IsDone;
                    }
                }
            }

            return progressLessonModel;
        }
    }
}
