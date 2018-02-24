using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Azure.Mobile.Server;
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
        private TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/Sections
        [Route("")]
        public IQueryable<Section> GetSections()
        {
            return db.Sections;
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

            return Ok(section);
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
            Section section = null;
            using (var dbContext = new Models.TeachMeBackendContext())
            {
                section = dbContext.DeleteSectionAndChildren(id);
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