namespace Linn.Api.Ifttt.Testing.Integration
{
    using System;
    using System.Net.Http;

    using Botwin;

    using Linn.Api.Ifttt.Service;
    using Linn.Api.Ifttt.Service.Modules;

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
                WebHost.CreateDefaultBuilder()
                    .ConfigureServices(
                        services =>
                            {
                                addDependencies(services);
                                services.AddBotwin(typeof(UserInfoModule).Assembly);
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
