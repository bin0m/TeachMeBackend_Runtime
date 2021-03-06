﻿using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
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
    [RoutePrefix("api/v{version:ApiVersion}/exerciseStudents")]
    [MobileAppController]
    [Authorize]
    public class ExerciseStudentsController : ApiController
    {
        private readonly TeachMeBackendContext _db = new TeachMeBackendContext();

        // GET: api/ExerciseStudents
        [Route("")]
        public IQueryable<ExerciseStudent> GetExerciseStudents()
        {
            return _db.ExerciseStudents;
        }

        // GET: api/ExerciseStudents/5
        [Route("{id}", Name = "GetExerciseStudentsById")]
        [ResponseType(typeof(ExerciseStudent))]
        public IHttpActionResult GetExerciseStudent(string id)
        {
            ExerciseStudent exerciseStudent = _db.ExerciseStudents.Find(id);
            if (exerciseStudent == null)
            {
                return NotFound();
            }

            return Ok(exerciseStudent);
        }

        
        [Route("~/api/v{version:ApiVersion}/exercises/{id}/exercisestudents")]
        public IQueryable<ExerciseStudent> GetByExercise(string id)
        {
            var exerciseStudents = _db.ExerciseStudents.Where(c => c.ExerciseId == id);

            return exerciseStudents;
        }

        // POST: api/ExerciseStudents
        [Route("")]
        [ResponseType(typeof(ExerciseStudent))]
        public IHttpActionResult PostExerciseStudent(ExerciseStudent exerciseStudent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (String.IsNullOrEmpty(exerciseStudent.Id))
            {
                exerciseStudent.Id = Guid.NewGuid().ToString("N");
            }       
    
            _db.ExerciseStudents.Add(exerciseStudent);

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ExerciseStudentExists(exerciseStudent.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetExerciseStudentsById", new { id = exerciseStudent.Id }, exerciseStudent);
        }


        // PUT: api/ExerciseStudents/5
        [Route("{id}")]
        [ResponseType(typeof(ExerciseStudent))]
        public IHttpActionResult PutExerciseStudent(string id, ExerciseStudent exerciseStudent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != exerciseStudent.Id)
            {
                return BadRequest();
            }

            var parentInDb = _db.ExerciseStudents
                .SingleOrDefault(p => p.Id == exerciseStudent.Id);

            if (parentInDb != null)
            {
                // to prevent error: "Modifying a column with the 'Identity' pattern is not supported. Column: 'CreatedAt'"
                exerciseStudent.CreatedAt = parentInDb.CreatedAt;

                // Update parent
                _db.Entry(parentInDb).CurrentValues.SetValues(exerciseStudent);   

                try
                {
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseStudentExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ExerciseStudent freshExerciseStudent = _db
                .ExerciseStudents
                .SingleOrDefault(ex => ex.Id == id);

            if (freshExerciseStudent == null)
            {
                return NotFound();
            }

            return Ok(freshExerciseStudent);
        }

        // DELETE: api/ExerciseStudents/5
        [Route("{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteExerciseStudent(string id)
        {
            ExerciseStudent exerciseStudent = _db.ExerciseStudents.Find(id);

            if (exerciseStudent == null)
            {
                return NotFound();
            }

            _db.ExerciseStudents.Remove(exerciseStudent);

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExerciseStudentExists(string id)
        {
            return _db.ExerciseStudents.Count(e => e.Id == id) > 0;
        }
    }
}