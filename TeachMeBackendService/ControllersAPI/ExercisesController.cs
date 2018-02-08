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
    [RoutePrefix("api/v{version:ApiVersion}/exercises")]
    [MobileAppController]
    public class ExercisesController : ApiController
    {
        private TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/Exercises
        [Route("")]
        public IQueryable<Exercise> GetExercises()
        {
            return db.Exercises;
        }

        // GET: api/Exercises/5
        [Route("{id}", Name = "GetExercisesById")]
        [ResponseType(typeof(Exercise))]
        public IHttpActionResult GetExercise(string id)
        {
            Exercise exercise = db.Exercises.Find(id);
            if (exercise == null)
            {
                return NotFound();
            }

            return Ok(exercise);
        }

        
        [Route("~/api/v{version:ApiVersion}/lessons/{id}/exercises")]
        public IQueryable<Exercise> GetBySection(string id)
        {
            var exercises = db.Exercises.Where(c => c.LessonId == id);

            return exercises;
        }

        // POST: api/Exercises
        [Route("")]
        [ResponseType(typeof(Exercise))]
        public IHttpActionResult PostExercise(Exercise exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            exercise.Id = Guid.NewGuid().ToString("N");
            if (exercise.Pairs != null)
            {
                foreach (var pair in exercise.Pairs)
                {
                    pair.Id = Guid.NewGuid().ToString("N");
                }
            }
            //Exercise newExercise = new Exercise
            //{
            //    Name = "TestName",
            //    Type = "TestType",
            //    LessonId = "c1x6409cf5444d8d866578ad3dd349nk",
            //    Id = Guid.NewGuid().ToString("N"),
            //    Pairs = new List<Pair> {
            //        new Pair{
            //            Value = "testValue",
            //            Equal = "testEqual",
            //            Id = Guid.NewGuid().ToString("N")
            //        } }
            //};

            db.Exercises.Add(exercise);


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                if (ExerciseExists(exercise.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw ex;
                }
            }

            return CreatedAtRoute("GetExercisesById", new { id = exercise.Id }, exercise);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExerciseExists(string id)
        {
            return db.Exercises.Count(e => e.Id == id) > 0;
        }
    }
}