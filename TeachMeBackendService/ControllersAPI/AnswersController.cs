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
    [RoutePrefix("api/v{version:ApiVersion}/answers")]
    [MobileAppController]
    [Authorize]
    public class AnswersController : ApiController
    {
        private TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/Answers
        [Route("")]
        public IQueryable<Answer> GetAnswers()
        {
            return db.Answers;
        }

        // GET: api/Answers/5
        [Route("{id}")]
        [ResponseType(typeof(Answer))]
        public IHttpActionResult GetAnswer(string id)
        {
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return NotFound();
            }

            return Ok(answer);
        }

        
        [Route("~/api/v{version:ApiVersion}/exercises/{id}/answers")]
        public IQueryable<Answer> GetByExercise(string id)
        {
            var answers = db.Answers.Where(c => c.ExerciseId == id);

            return answers;
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