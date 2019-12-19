using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.PlatformAbstractions;

namespace App.WebApi.IntegrationTest.Infrastructure
{


    public sealed class TestServerFixture : IDisposable
    {
        public HttpClient Client { get; }
        private static readonly TestServer TestServer;

        static TestServerFixture()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(GetContentRootPath())
                .UseEnvironment("Testing")
                .UseStartup<Startup>();

            TestServer = new TestServer(builder);
        }
        public TestServerFixture()
        {
            Client = TestServer.CreateClient();
        }

        private static string GetContentRootPath()
        {
            var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            var relativePathToHostProject = @"..\..\..\..\..\App.WebApi";
            return Path.Combine(testProjectPath, relativePathToHostProject);
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}

