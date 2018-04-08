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
    [RoutePrefix("tables/course")]
    public class CourseController : TableController<Course>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Course>(context, Request);
        }

        // GET tables/Course
        [Route("")]
        public IQueryable<Course> GetAllCourse()
        {
            return Query();
        }

        // GET tables/Course/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetCourse")]
        public SingleResult<Course> GetCourse(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Course/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<Course> PatchCourse(string id, Delta<Course> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Course
        [Route("")]
        public async Task<IHttpActionResult> PostCourse(Course item)
        {
            Course current = await InsertAsync(item);
            return CreatedAtRoute("GetCourse", new { id = current.Id }, current);
        }

        // DELETE tables/Course/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteCourse(string id)
        {
            return DeleteAsync(id);
        }

        // GET tables/Course/48D68C86-6EA6-4C25-AA33-223FC9A27959/progress
        [Route("{id}/progress")]
        [ResponseType(typeof(ProgressCourseModel))]
        public IHttpActionResult GetCourseProgress(string id)
        {
            using (var db = new TeachMeBackendContext())
            {
                Course course = db.Courses.Find(id);
                if (course == null)
                {
                    return NotFound();
                }
            }

            //Calculates progress for all exercises under this course for the current user
            ProgressCourseModel progress = CalculateCourseProgress(id);

            return Ok(progress);
        }

        //Calculates progress for all sections under this course for the current user
        private ProgressCourseModel CalculateCourseProgress(string id)
        {
            ProgressCourseModel progressCourseModel = new ProgressCourseModel();

            using (var db = new TeachMeBackendContext())
            {
                var sections = db.Sections.Where(c => c.CourseId == id).Include(c => c.SectionProgresses);
                progressCourseModel.SectionsNumber = sections.Count();
                if (User is ClaimsPrincipal claimsPrincipal)
                {
                    var userId = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid).Value;
                    progressCourseModel.SectionsDone =
                        sections.Count(c => c.SectionProgresses.Any(p => p.UserId == userId && p.IsDone));
                }
            }

            return progressCourseModel;
        }
    }
}
