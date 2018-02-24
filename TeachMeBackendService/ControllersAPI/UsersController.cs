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
using Microsoft.AspNet.Identity.Owin;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.ControllersAPI
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:ApiVersion}/users")]
    [MobileAppController]
    public class UsersController : ApiController
    {
        TeachMeBackendContext dbContext
        {
            get
            {
                return Request.GetOwinContext().Get<TeachMeBackendContext>();
            }
        }
        //private TeachMeBackendContext db = new TeachMeBackendContext();

        //// GET: api/Users
        //[Route("")]
        //public IQueryable<User> GetUsers()
        //{
        //    return db.Users;
        //}

        //// GET: api/Users/5
        //[Route("{id}")]
        //[ResponseType(typeof(User))]
        //public IHttpActionResult GetUser(string id)
        //{
        //    User user = db.Users.Find(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(user);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool UserExists(string id)
        //{
        //    return db.Users.Count(e => e.Id == id) > 0;
        //}
    }
}