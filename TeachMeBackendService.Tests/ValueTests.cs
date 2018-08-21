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
        public void TestGet()
        {
           // var result = valuesController.Get();
           // Assert.AreEqual(result, "Hello from localhost");
        }
    }
}
