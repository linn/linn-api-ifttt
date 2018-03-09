namespace Linn.Api.Ifttt
{
    using Botwin;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Api.Ifttt.Service;
    using Linn.Api.Ifttt.Service.Factories;
    using Linn.Api.Ifttt.Service.Modules;
    using Linn.Common.Proxy;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBotwin(typeof(UserInfoModule).Assembly);

            services.AddSingleton<IRestClient>(i => new RestClient(10000));
            services.AddSingleton(typeof(IUserResourceFactory), typeof(UserResourceFactory));
            services.AddSingleton(typeof(ILinnApiActions), typeof(LinnApiActions));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler(ExceptionHandlers.Handlers);
            app.UseBotwin();
        }
    }
}