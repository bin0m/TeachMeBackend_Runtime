using NUnit.Framework;
using TeachMeBackendService;

namespace TeachMeBackendService.Tests
{
       [TestFixture]
        public class SimpleTests
    {
            private readonly ControllersTables.ValuesController valuesController;

            public SimpleTests()
            {
                valuesController = new ControllersTables.ValuesController();
            }

            [Test]
            public void ReturnFalseGivenValueOf1()
            {
                var result = valuesController.Get();

                Assert.AreEqual(result, "Hello from localhost");
            }
        }
   
}
