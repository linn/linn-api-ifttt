namespace Linn.Api.Ifttt.Testing.Integration
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;

    using Botwin;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;

    public class TestClient
    {
        private string accessToken;

        private Assembly assembly;

        public TestClient WithAssembly(Assembly serviceAssembly)
        {
            this.assembly = serviceAssembly;
            return this;
        }

        public TestClient WithAccessToken(string token)
        {
            this.accessToken = token;
            return this;
        }

        public HttpClient With(Action<IServiceCollection> addDependencies)
        {
            var server = new TestServer(
                WebHost.CreateDefaultBuilder()
                    .ConfigureServices(
                        services =>
                            {
                                addDependencies(services);
                                services.AddBotwin(this.assembly);
                            })
                    .Configure(app => app.UseBotwin()));

            var client = server.CreateClient();

            if (!string.IsNullOrEmpty(this.accessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
            }

            return client;
        }
    }
}
