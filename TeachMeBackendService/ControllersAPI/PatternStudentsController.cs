﻿using System;
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
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.ControllersAPI
{

    [MobileAppController]
    public class PatternStudentsController : ApiController
    {
        private TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/PatternStudents
        public IQueryable<PatternStudent> GetPatternStudents()
        {
            return db.PatternStudents;
        }

        // GET: api/PatternStudents/5
        [ResponseType(typeof(PatternStudent))]
        public IHttpActionResult GetPatternStudent(string id)
        {
            PatternStudent patternStudent = db.PatternStudents.Find(id);
            if (patternStudent == null)
            {
                return NotFound();
            }

            return Ok(patternStudent);
        }


        [Route("~/api/patterns/{id}/patternStudents")]
        public IQueryable<PatternStudent> GetBySection(string id)
        {
            var patternStudents = db.PatternStudents.Where(c => c.PatternId == id);

            return patternStudents;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatternStudentExists(string id)
        {
            return db.PatternStudents.Count(e => e.Id == id) > 0;
        }
    }
}