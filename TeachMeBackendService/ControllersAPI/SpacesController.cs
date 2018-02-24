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
    [RoutePrefix("api/v{version:ApiVersion}/spaces")]
    [MobileAppController]
    [Authorize]
    public class SpacesController : ApiController
    {
        private TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/Spaces
        [Route("")]
        public IQueryable<Space> GetSpaces()
        {
            return db.Spaces;
        }

        // GET: api/Spaces/5
        [Route("{id}")]
        [ResponseType(typeof(Space))]
        public IHttpActionResult GetSpace(string id)
        {
            Space space = db.Spaces.Find(id);
            if (space == null)
            {
                return NotFound();
            }

            return Ok(space);
        }

        
        [Route("~/api/v{version:ApiVersion}/exercises/{id}/spaces")]
        public IQueryable<Space> GetByExercise(string id)
        {
            var spaces = db.Spaces.Where(c => c.ExerciseId == id);

            return spaces;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SpaceExists(string id)
        {
            return db.Spaces.Count(e => e.Id == id) > 0;
        }
    }
}