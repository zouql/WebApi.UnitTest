namespace WebUnitTest
{
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTest : BaseHost
    {
        [TestMethod]
        public async Task TestMethodAsync()
        {
            var respose = await Client.GetAsync("api/Home/Index");

            Assert.AreEqual(respose.StatusCode, HttpStatusCode.OK);
        }
    }
}
