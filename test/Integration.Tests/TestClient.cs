namespace Linn.Api.Ifttt.Testing.Integration
{
    using System;
    using System.Net.Http;
    using System.Reflection;

    using Botwin;

    using Linn.Api.Ifttt.Service;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;

    public class TestClient
    {
        private Assembly assembly;

        public TestClient WithAssembly(Assembly serviceAssembly)
        {
            this.assembly = serviceAssembly;
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
                    .Configure(app =>
                        {
                            app.UseExceptionHandler(ExceptionHandlers.Handlers);
                            app.UseBotwin();
                        }));

            var client = server.CreateClient();

            return client;
        }
    }
}
