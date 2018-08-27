using NUnit.Framework;
using TeachMeBackendService.ControllersTables;

namespace TeachMeBackendService.Tests
{
    [TestFixture]
    public class ValueTests
    {
        private readonly ValuesController _valuesController;

        public ValueTests()
        {
            _valuesController = new ValuesController();
        }

        [Test]
        public void TestPost()
        {
           var result = _valuesController.Post();
           Assert.AreEqual("Hello World!", result);
        }

       

    }
}
