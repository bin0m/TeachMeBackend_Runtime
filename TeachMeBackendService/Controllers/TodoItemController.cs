using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;
using System.Collections.Generic;
using System;
using Microsoft.Web.Http;

namespace TeachMeBackendService.Controllers
{
 //   [ApiVersion("1.0")]   
 //   [RoutePrefix("api/v{version:ApiVersion}/todoitem")]
    [ApiVersionNeutral]
    public class TodoItemController : TableController<TodoItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TeachMeBackendContext context = new TeachMeBackendContext();
            DomainManager = new EntityDomainManager<TodoItem>(context, Request);
        }

        // GET tables/TodoItem
  //      [Route("")]
        public IQueryable<TodoItem> GetAllTodoItems()
        {
            var query = Query();
            if (query.Count<TodoItem>() == 0)
            {
                List<TodoItem> todoItems = new List<TodoItem>
                {
                    new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
                    new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
                };

                using (TeachMeBackendContext context = new TeachMeBackendContext())
                {
                    foreach (TodoItem todoItem in todoItems)
                    {
                        context.Set<TodoItem>().Add(todoItem);
                    }
                    context.SaveChanges();
                }
                    
            }
            return query;
        }

        // GET tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
//        [Route("{id}", Name = "GetTodoItem")]
        public SingleResult<TodoItem> GetTodoItem(string id)
        {
             return Lookup(id);
        }

        // PATCH tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
 //       [Route("{id}")]
        public Task<TodoItem> PatchTodoItem(string id, Delta<TodoItem> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/TodoItem
//        [Route("")]
        public async Task<IHttpActionResult> PostTodoItem(TodoItem item)
        {
            TodoItem current = await InsertAsync(item);
            return CreatedAtRoute("GetTodoItem", new { id = current.Id }, current);
        }

        // DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
  //      [Route("{id}")]
        public Task DeleteTodoItem(string id)
        {
            return DeleteAsync(id);
        }
    }
}