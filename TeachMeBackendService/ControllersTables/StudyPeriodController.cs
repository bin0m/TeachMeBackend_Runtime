﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Web.Http;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;
   
namespace TeachMeBackendService.ControllersTables
{
    [Authorize]
    [ApiVersionNeutral]
    [RoutePrefix("tables/StudyPeriod")]
    public class StudyPeriodController : TableController<StudyPeriod>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<StudyPeriod>(context, Request);
        }

        // GET tables/StudyPeriod
        public IQueryable<StudyPeriod> GetAllStudyPeriod()
        {
            return Query(); 
        }

        // GET tables/StudyPeriod/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<StudyPeriod> GetStudyPeriod(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/StudyPeriod/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<StudyPeriod> PatchStudyPeriod(string id, Delta<StudyPeriod> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/StudyPeriod
        public async Task<IHttpActionResult> PostStudyPeriod(StudyPeriod item)
        {
            StudyPeriod current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/StudyPeriod/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteStudyPeriod(string id)
        {
             return DeleteAsync(id);
        }
    }
}
