namespace Linn.Api.Ifttt
{
    using Carter;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Api.Ifttt.Service;
    using Linn.Api.Ifttt.Service.Factories;
    using Linn.Common.Proxy;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCarter();

            services.AddSingleton<IRestClient, RestClient>();
            services.AddSingleton<IUserResourceFactory, UserResourceFactory>();
            services.AddSingleton<ILinnApiActions, LinnApiActions>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler(ExceptionHandlers.Handlers);
            app.UseCarter();
        }
    }
}