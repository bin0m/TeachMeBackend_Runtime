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
    [RoutePrefix("api/v{version:ApiVersion}/exercisestudents")]
    [MobileAppController]
    [Authorize]
    public class ExerciseStudentsController : ApiController
    {
        private TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/ExerciseStudents
        [Route("")]
        public IQueryable<ExerciseStudent> GetExerciseStudents()
        {
            return db.ExerciseStudents;
        }

        // GET: api/ExerciseStudents/5
        [Route("{id}")]
        [ResponseType(typeof(ExerciseStudent))]
        public IHttpActionResult GetExerciseStudent(string id)
        {
            ExerciseStudent exerciseStudent = db.ExerciseStudents.Find(id);
            if (exerciseStudent == null)
            {
                return NotFound();
            }

            return Ok(exerciseStudent);
        }

        
        [Route("~/api/v{version:ApiVersion}/exercises/{id}/exercisestudents")]
        public IQueryable<ExerciseStudent> GetByExercise(string id)
        {
            var exerciseStudents = db.ExerciseStudents.Where(c => c.ExerciseId == id);

            return exerciseStudents;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExerciseStudentExists(string id)
        {
            return db.ExerciseStudents.Count(e => e.Id == id) > 0;
        }
    }
}