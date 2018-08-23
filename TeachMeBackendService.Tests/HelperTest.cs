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

        [Test]
        public void Base64EncodeDecodeTest3()
        {
            string data = "Dv1HX5dzyTh8gA2YsLI+yWEgh9lnGKPpRDDEYYvm42f jBFziUYfcpvA9g8GXQuU9srY3mhfsZkCDHN0x5n1gliOai5TSjmd5Hh+9UyhvNWE+D8HoUpcFXWoQXvy/if2r25m+ZWi3cqgXkkBOcal3W1ePMtU4ln18NcWyIZ0tEFo=Gghh/e3Gsbj1+4RR9Lh2aR/xJl35HWiHqlPIeSUqE9D7uDCVTAwNce/dGL3Ew7uJPfJ6Pgr70wD3zgu3stw0Zmzayax0hiDtGwcQCxVIER08wqGANK9C2Q7PYJkNTNtiTo6ehKWbdV4Z+/U+TEYyQfpQTDbAFYk/vVpsdjp0Lmc=";
            string encoded = Helper.Base64Encode(data);
            Assert.AreNotEqual(data, encoded);
            string decoded = Helper.Base64Decode(encoded);
            Assert.AreEqual(data, decoded);
        }
    }
}
