using Microsoft.Azure.Mobile.Server;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using TeachMeBackendService.ControllersTables;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.Tests
{
    [TestFixture]
    public class TodoItemTests
    {
        private readonly TodoItemController _controller;


        public TodoItemTests()
        {
            //AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory()); // See 1*
            //WebApiConfig.Register(); // See 2*

            _controller = new TodoItemController();
            _controller.SetDomainManager(
                new EntityDomainManager<TodoItem>(
                    new TeachMeBackendContext(),
                    new HttpRequestMessage())
                );
           
        }


        [Test]
        public void TestGet()
        {
            var result = _controller.GetAllTodoItems().ToList().Count;
            Assert.AreEqual(2, result);
        }
    }
}
