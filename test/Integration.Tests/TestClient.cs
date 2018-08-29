using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Linn.Api.Ifttt.Testing.Integration
{
    using System;
    using System.Net.Http;

    using Carter;

    using Linn.Api.Ifttt.Service;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;

    public class TestClient
    {
        public HttpClient With(Action<IServiceCollection> addDependencies)
        {
            var server = new TestServer(
                WebHost.CreateDefaultBuilder().ConfigureServices(
                    services =>
                        {
                            services.AddCarter();
                            addDependencies(services);
                        }).Configure(
                    app =>
                        {
                            app.UseExceptionHandler(ExceptionHandlers.Handlers);
                            app.UseCarter();
                        }));

            var client = server.CreateClient();

            return client;
        }
    }
}
