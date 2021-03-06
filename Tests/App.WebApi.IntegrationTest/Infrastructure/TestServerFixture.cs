﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;

namespace App.WebApi.IntegrationTest.Infrastructure
{
    public sealed class TestServerFixture : IDisposable
    {
        public HttpClient Client { get; }
        private static readonly TestServer TestServer;

        static TestServerFixture()
        {
            var builder = new WebHostBuilder()
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