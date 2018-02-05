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
    [RoutePrefix("api/v{version:ApiVersion}/commentratings")]
    [MobileAppController]
    public class CommentRatingsController : ApiController
    {
        private TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/CommentRatings
        [Route("")]
        public IQueryable<CommentRating> GetCommentRatings()
        {
            return db.CommentRatings;
        }

        // GET: api/CommentRatings/5
        [Route("{id}")]
        [ResponseType(typeof(CommentRating))]
        public IHttpActionResult GetCommentRating(string id)
        {
            CommentRating CommentRating = db.CommentRatings.Find(id);
            if (CommentRating == null)
            {
                return NotFound();
            }

            return Ok(CommentRating);
        }


        [Route("~/api/v{version:ApiVersion}/comments/{id}/CommentRatings")]
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