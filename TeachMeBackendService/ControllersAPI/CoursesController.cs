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
    [RoutePrefix("api/v{version:ApiVersion}/courses")]
    [MobileAppController]
    public class CoursesController : ApiController
    {
        private TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/Courses
        [Route("")]
        public IQueryable<Course> GetCourses()
        {
            return db.Courses;
        }

        // GET: api/Courses/5
        [Route("{id}")]
        [ResponseType(typeof(Course))]
        public IHttpActionResult GetCourse(string id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        // PUT: api/Courses/5
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCourse(string id, Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course.Id)
            {
                return BadRequest();
            }

            db.Entry(course).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // POST: api/Courses
        [Route("")]
        [ResponseType(typeof(Course))]
        public IHttpActionResult PostCourse(Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Courses.Add(course);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CourseExists(course.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = course.Id }, course);
        }

        // DELETE: api/Courses/5
        [Route("{id}")]
        [ResponseType(typeof(Course))]
        public IHttpActionResult DeleteCourse(string id)
        {
            Course course = null;
            using (var dbContext = new Models.TeachMeBackendContext())
            {
                course = dbContext.DeleteCourseAndChildren(id);
                if (course == null)
                {
                    return NotFound();
                }
                dbContext.SaveChanges();
            }

            return Ok(course);
        }

        [Route("~/api/v{version:ApiVersion}/users/{usesrId}/courses")]
        public IQueryable<Course> GetByUser(string usesrId)
        {
            var courses = db.Courses.Where(c => c.UserId == usesrId);

            return courses;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseExists(string id)
        {
            return db.Courses.Count(e => e.Id == id) > 0;
        }
    }
}