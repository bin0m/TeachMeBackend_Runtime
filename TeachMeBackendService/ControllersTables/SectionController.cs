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
    [RoutePrefix("tables/section")]
    public class SectionController : TableController<Section>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Section>(context, Request);
        }

        // GET tables/Section
        public IQueryable<Section> GetAllSection()
        {
            return Query(); 
        }

        // GET tables/Section/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Section> GetSection(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Section/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Section> PatchSection(string id, Delta<Section> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Section
        public async Task<IHttpActionResult> PostSection(Section item)
        {
            Section current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Section/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteSection(string id)
        {
             return DeleteAsync(id);
        }

        // GET tables/Section/48D68C86-6EA6-4C25-AA33-223FC9A27959/progress
        [Route("{id}/progress")]
        [ResponseType(typeof(ProgressSectionModel))]
        public IHttpActionResult GetSectionProgress(string id)
        {
            using (var db = new TeachMeBackendContext())
            {
                Section section = db.Sections.Find(id);
                if (section == null)
                {
                    return NotFound();
                }
            }

            //Calculates progress for all lessons under this section for the current user
            ProgressSectionModel progress = CalculateSectionProgress(id);

            return Ok(progress);
        }

        //Calculates progress for all lessons under this section for the current user
        private ProgressSectionModel CalculateSectionProgress(string id)
        {
            ProgressSectionModel progressSectionModel = new ProgressSectionModel();

            using (var db = new TeachMeBackendContext())
            {
                var lessons = db.Lessons.Where(l => l.SectionId == id).Include(l => l.LessonProgresses);
                progressSectionModel.LessonsNumber = lessons.Count();
                if (User is ClaimsPrincipal claimsPrincipal)
                {
                    var userId = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid).Value;
                    progressSectionModel.LessonsDone =
                        lessons.Count(l => l.LessonProgresses.Any(p => p.UserId == userId && p.IsDone));
                    var sectionProgress =
                        db.SectionProgresses.FirstOrDefault(p => p.UserId == userId && p.SectionId == id);
                    if (sectionProgress != null)
                    {
                        progressSectionModel.IsDone = sectionProgress.IsDone;
                        progressSectionModel.IsStarted = sectionProgress.IsDone;
                    }
                }
            }

            return progressSectionModel;
        }
    }
}
