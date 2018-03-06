﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Web.Http;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.Controllers
{
    [Authorize]
    [ApiVersionNeutral]
    [RoutePrefix("tables/section")]
    public class SectionController : TableController<Section>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Section>(context, Request);
        }

        // GET tables/Section
        public IQueryable<Section> GetAllSection()
        {
            return Query(); 
        }

        // GET tables/Section/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Section> GetSection(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Section/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Section> PatchSection(string id, Delta<Section> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Section
        public async Task<IHttpActionResult> PostSection(Section item)
        {
            Section current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Section/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteSection(string id)
        {
             return DeleteAsync(id);
        }
    }
}