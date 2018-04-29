using System.Linq;
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
    [RoutePrefix("tables/GroupUser")]
    public class GroupUserController : TableController<GroupUser>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<GroupUser>(context, Request);
        }

        // GET tables/GroupUser
        public IQueryable<GroupUser> GetAllGroupUser()
        {
            return Query(); 
        }

        // GET tables/GroupUser/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<GroupUser> GetGroupUser(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/GroupUser/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<GroupUser> PatchGroupUser(string id, Delta<GroupUser> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/GroupUser
        public async Task<IHttpActionResult> PostGroupUser(GroupUser item)
        {
            GroupUser current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/GroupUser/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteGroupUser(string id)
        {
             return DeleteAsync(id);
        }
    }
}
