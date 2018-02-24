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
    [RoutePrefix("api/v{version:ApiVersion}/pairs")]
    [MobileAppController]
    [Authorize]
    public class PairsController : ApiController
    {
        private TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/Pairs
        [Route("")]
        public IQueryable<Pair> GetPairs()
        {
            return db.Pairs;
        }

        // GET: api/Pairs/5
        [Route("{id}")]
        [ResponseType(typeof(Pair))]
        public IHttpActionResult GetPair(string id)
        {
            Pair pair = db.Pairs.Find(id);
            if (pair == null)
            {
                return NotFound();
            }

            return Ok(pair);
        }

        
        [Route("~/api/v{version:ApiVersion}/exercises/{id}/pairs")]
        public IQueryable<Pair> GetByExercise(string id)
        {
            var pairs = db.Pairs.Where(c => c.ExerciseId == id);

            return pairs;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PairExists(string id)
        {
            return db.Pairs.Count(e => e.Id == id) > 0;
        }
    }
}