using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
    [RoutePrefix("api/v{version:ApiVersion}/sections")]
    [MobileAppController]
    [Authorize]
    public class SectionsController : ApiController
    {
        private readonly TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/Sections
        [Route("")]
        public IEnumerable<Section> GetSections()
        {
            var all = db.Sections.OrderBy(x => x.CreatedAt).ToList();
            all.ForEach(x => x.Progress = CalculateSectionProgress(x.Id));
            return all;
        }

        // GET: api/Sections/5
        [Route("{id}")]
        [ResponseType(typeof(Section))]        
        public IHttpActionResult GetSection(string id)
        {
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return NotFound();
            }

            //Calculates progress for all lessons under this section for the current user
            section.Progress = CalculateSectionProgress(id);

            return Ok(section);
        }

        //Calculates progress for all lessons under this section for the current user
        private ProgressSectionModel CalculateSectionProgress(string id)
        {
            ProgressSectionModel progressSectionModel = new ProgressSectionModel();
            var lessons = db.Lessons.Where(l => l.SectionId == id).Include(l => l.LessonProgresses);
            progressSectionModel.LessonsNumber = lessons.Count();
            if (User is ClaimsPrincipal claimsPrincipal)
            {
                var userId = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid).Value;
                progressSectionModel.LessonsDone =
                    lessons.Count(l => l.LessonProgresses.Any(p => p.UserId == userId && p.IsDone));
                var sectionProgress = db.SectionProgresses.FirstOrDefault(p => p.UserId == userId && p.SectionId == id);
                if (sectionProgress != null)
                {
                    progressSectionModel.IsDone = sectionProgress.IsDone;
                    progressSectionModel.IsStarted = sectionProgress.IsDone;
                }
            }
            return progressSectionModel;
        }

        // PUT: api/Sections/5
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSection(string id, Section section)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != section.Id)
            {
                return BadRequest();
            }

            db.Entry(section).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SectionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Sections
        [Route("")]
        [ResponseType(typeof(Section))]
        public IHttpActionResult PostSection(Section section)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (String.IsNullOrEmpty(section.Id))
            {
                section.Id = Guid.NewGuid().ToString("N");
            }

            db.Sections.Add(section);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SectionExists(section.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = section.Id }, section);
        }

        // DELETE: api/Sections/5
        [Route("{id}")]
        [ResponseType(typeof(Section))]
        public IHttpActionResult DeleteSection(string id)
        {
            using (var dbContext = new TeachMeBackendContext())
            {
                var section = dbContext.DeleteSectionAndChildren(id);
                if (section == null)
                {
                    return NotFound();
                }
                dbContext.SaveChanges();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("~/api/v{version:ApiVersion}/courses/{id}/sections")]
        public IQueryable<Section> GetByCourse(string id)
        {
            var sections = db.Sections.Where(c => c.CourseId == id);

            return sections;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SectionExists(string id)
        {
            return db.Sections.Count(e => e.Id == id) > 0;
        }
    }
}