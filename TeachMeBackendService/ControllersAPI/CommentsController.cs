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
    public class CommentsController : ApiController
    {
        private TeachMeBackendContext db = new TeachMeBackendContext();

        // GET: api/Comments
        public IQueryable<Comment> GetComments()
        {
            return db.Comments;
        }

        // GET: api/Comments/5
        [ResponseType(typeof(Comment))]
        public IHttpActionResult GetComment(string id)
        {
            Comment patternStudent = db.Comments.Find(id);
            if (patternStudent == null)
            {
                return NotFound();
            }

            return Ok(patternStudent);
        }


        [Route("~/api/patterns/{id}/comments")]
        public IQueryable<Comment> GetBySection(string id)
        {
            var comment = db.Comments.Include(c => c.User).Where(c => c.PatternId == id);

            return comment;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentExists(string id)
        {
            return db.Comments.Count(e => e.Id == id) > 0;
        }
    }
}