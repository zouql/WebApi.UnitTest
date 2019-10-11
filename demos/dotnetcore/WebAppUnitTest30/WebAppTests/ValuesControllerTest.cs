namespace WebAppUnitTest30.WebAppTests
{
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ValuesControllerTest : BaseHost
    {
        public ValuesControllerTest()
        {
            SignInForCookieAsync().Wait();
        }

        [TestMethod]
        public async Task TestMethodAsync()
        {
            var respose = await GetAsync("api/Values/Get?id=1");

            Assert.AreEqual(respose.StatusCode, HttpStatusCode.OK);
        }
    }
}
