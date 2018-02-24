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
    [RoutePrefix("api/v{version:ApiVersion}/lessons")]
    [MobileAppController]
    [Authorize]
    public class LessonsController : ApiController
    {
        private TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/Lessons
        [Route("")]
        public IQueryable<Lesson> GetLessons()
        {
            return db.Lessons;
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

            return Ok(lesson);
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

        private bool LessonExists(string id)
        {
            return db.Lessons.Count(e => e.Id == id) > 0;
        }
    }
}