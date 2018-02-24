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
    [RoutePrefix("api/v{version:ApiVersion}/studentcourses")]
    [MobileAppController]
    [Authorize]
    public class StudentCoursesController : ApiController
    {
        private TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/StudentCourses
        [Route("")]
        public IQueryable<StudentCourse> GetStudentCourses()
        {
            return db.StudentCourses;
        }

        // GET: api/StudentCourses/5
        [Route("{id}")]
        [ResponseType(typeof(StudentCourse))]
        public IHttpActionResult GetStudentCourse(string id)
        {
            StudentCourse studentCourse = db.StudentCourses.Find(id);
            if (studentCourse == null)
            {
                return NotFound();
            }

            return Ok(studentCourse);
        }

        
        [Route("~/api/v{version:ApiVersion}/users/{id}/studentcourses")]
        public IQueryable<StudentCourse> GetByExercise(string id)
        {
            var studentCourses = db.StudentCourses.Where(c => c.UserId == id);

            return studentCourses;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentCourseExists(string id)
        {
            return db.StudentCourses.Count(e => e.Id == id) > 0;
        }
    }
}