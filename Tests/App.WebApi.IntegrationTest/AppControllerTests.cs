using App.WebApi.IntegrationTest.Infrastructure;

namespace App.WebApi.IntegrationTest
{
    public class AppControllerTests : ControllerTestBase
    {
        private readonly TestServerFixture _fixture;

        public AppControllerTests(TestServerFixture fixture)
            : base(fixture)
        {
            _fixture = fixture;
        }
    }
}