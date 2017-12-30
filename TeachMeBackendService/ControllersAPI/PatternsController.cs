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
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.ControllersAPI
{
    public class PatternsController : ApiController
    {
        private TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/Patterns
        public IQueryable<Pattern> GetPatterns()
        {
            return db.Patterns;
        }

        // GET: api/Patterns/5
        [ResponseType(typeof(Pattern))]
        public IHttpActionResult GetPattern(string id)
        {
            Pattern pattern = db.Patterns.Find(id);
            if (pattern == null)
            {
                return NotFound();
            }

            return Ok(pattern);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatternExists(string id)
        {
            return db.Patterns.Count(e => e.Id == id) > 0;
        }
    }
}