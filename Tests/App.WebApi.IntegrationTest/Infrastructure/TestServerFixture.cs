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
               //.UseContentRoot(GetContentRootPath())
                .UseEnvironment("Testing")
                .UseStartup<Startup>();

            TestServer = new TestServer(builder);
        }
        public TestServerFixture()
        {
            Client = TestServer.CreateClient();
        }
        public void Dispose()
        {
            Client.Dispose();
        }
    }
}

