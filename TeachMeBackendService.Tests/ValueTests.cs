using NUnit.Framework;
using TeachMeBackendService.ControllersTables;

namespace TeachMeBackendService.Tests
{
    [TestFixture]
    public class ValueTests
    {
        private readonly ValuesController valuesController;

        public ValueTests()
        {
            valuesController = new ValuesController();
        }

        [Test]
        public void TestPost()
        {
           var result = valuesController.Post();
           Assert.AreEqual(result, "Hello World!");
        }

       

    }
}
