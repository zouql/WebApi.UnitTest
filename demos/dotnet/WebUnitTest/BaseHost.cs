namespace WebUnitTest
{
    using WebApi.UnitTest.DotNet;

    public class BaseHost : BaseTestHost
    {
        public BaseHost()
        {
            StartHost<Web.Startup>();
        }
    }
}
