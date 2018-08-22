using NUnit.Framework;
using TeachMeBackendService.Logic;

namespace TeachMeBackendService.Tests
{
    [TestFixture]
    public class HelperTest
    {
        [Test]
        public void Base64EncodeDecodeTest1()
        {
            string data = "myData12";
            string encoded = Helper.Base64Encode(data);
            Assert.AreNotEqual(data, encoded);
            string decoded = Helper.Base64Decode(encoded);
            Assert.AreEqual(data, decoded);
        }

        [Test]
        public void Base64EncodeDecodeTest2()
        {
            string data = "H#J^621)(*&!^>+?=";
            string encoded = Helper.Base64Encode(data);
            Assert.AreNotEqual(data, encoded);
            string decoded = Helper.Base64Decode(encoded);
            Assert.AreEqual(data, decoded);
        }
    }
}
