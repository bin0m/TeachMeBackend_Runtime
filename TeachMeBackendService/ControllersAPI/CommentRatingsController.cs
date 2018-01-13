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
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.ControllersAPI
{

    [MobileAppController]
    public class CommentRatingsController : ApiController
    {
        private TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/CommentRatings
        public IQueryable<CommentRating> GetCommentRatings()
        {
            return db.CommentRatings;
        }

        // GET: api/CommentRatings/5
        [ResponseType(typeof(CommentRating))]
        public IHttpActionResult GetCommentRating(string id)
        {
            CommentRating patternStudent = db.CommentRatings.Find(id);
            if (patternStudent == null)
            {
                return NotFound();
            }

            return Ok(patternStudent);
        }


        [Route("~/api/comments/{id}/CommentRatings")]
        public IQueryable<CommentRating> GetBySection(string id)
        {
            var CommentRating = db.CommentRatings.Where(c => c.CommentId == id);

            return CommentRating;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentRatingExists(string id)
        {
            return db.CommentRatings.Count(e => e.Id == id) > 0;
        }
    }
}