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
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:ApiVersion}/section2")]
    [Authorize]
    public class Section2Controller : TableController<Section2>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<Section2>(context, Request);
        }

        // GET tables/Section2
        [Route("")]
        public IQueryable<Section2> GetAllSection2()
        {
            return Query(); 
        }

        // GET tables/Section2/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}", Name = "GetSection2")]
        public SingleResult<Section2> GetSection2(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Section2/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task<Section2> PatchSection2(string id, Delta<Section2> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Section2
        [Route("")]
        public async Task<IHttpActionResult> PostSection2(Section2 item)
        {
            Section2 current = await InsertAsync(item);
            return CreatedAtRoute("GetSection2", new { id = current.Id }, current);
        }

        // DELETE tables/Section2/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("{id}")]
        public Task DeleteSection2(string id)
        {
             return DeleteAsync(id);
        }
    }
}
