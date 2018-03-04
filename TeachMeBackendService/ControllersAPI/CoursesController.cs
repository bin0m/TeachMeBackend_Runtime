﻿using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Web.Http;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.ControllersAPI
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:ApiVersion}/courses")]
    [MobileAppController]
    [Authorize]
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

        // DELETE: api/Courses/5
        [Route("{id}")]
        [ResponseType(typeof(Course))]
        public IHttpActionResult DeleteCourse(string id)
        {
            using (var dbContext = new TeachMeBackendContext())
            {
                var course = dbContext.DeleteCourseAndChildren(id);
                if (course == null)
                {
                    return NotFound();
                }
                dbContext.SaveChanges();
            }

            return StatusCode(HttpStatusCode.NoContent); 
        }

        [Route("~/api/v{version:ApiVersion}/users/{usesrId}/courses")]
        public IQueryable<Course> GetByUserId(string usesrId, bool isTeacher=false)
        {
            if (isTeacher)
            {
                var courses = db.Courses.Where(c => c.UserId == usesrId);
                return courses;
            }

            var query = from course in db.Courses
                        where course.StudentCourses.Any(c => c.UserId == usesrId)
                        select course;

            return query;

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