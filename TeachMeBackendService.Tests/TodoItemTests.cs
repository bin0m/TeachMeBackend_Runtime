using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using TeachMeBackendService.ControllersTables;

namespace TeachMeBackendService.Tests
{
    [TestFixture]
    public class TodoItemTests
    {
        private readonly TodoItemController _controller;


        //public TodoItemTests()
        //{
        //    AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory()); // See 1*
        //    WebApiConfig.Register(); // See 2*

        //    _controller = new TodoItemController();
        //    var context = new mdaviestestContext();
        //    _controller.DomainManager = (
        //        new EntityDomainManager<TodoItem>(
        //            context,
        //            new HttpRequestMessage(),
        //            new ApiServices(new HttpConfiguration())
        //        )
        //    );
        //}


        [Test]
        public void TestGet()
        {
            var result = _controller.GetAllTodoItems().ToList().Count;
            Assert.AreEqual(2, result);
        }
    }
}
