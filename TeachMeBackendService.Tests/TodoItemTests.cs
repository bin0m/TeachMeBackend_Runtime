using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachMeBackendService.ControllersTables;

namespace TeachMeBackendService.Tests
{
    [TestFixture]
    public class TodoItemTests
    {
        private readonly TodoItemController _controller;    
        [Test]
        public void TestGet()
        {
            //var result = _controller.GetAllTodoItems().ToList().Count;
            //Assert.AreEqual(2, result);
        }
    }
}
