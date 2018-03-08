namespace Linn.Api.Ifttt
{
    using System.Net;

    using Botwin;
    using Botwin.Response;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Api.Ifttt.Resources.Ifttt;
    using Linn.Api.Ifttt.Service;
    using Linn.Api.Ifttt.Service.Factories;
    using Linn.Api.Ifttt.Service.Modules;
    using Linn.Common.Proxy;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBotwin(typeof(UserInfoModule).Assembly);

            services.AddSingleton<IUserResourceFactory>(
                i => new UserResourceFactory());
            services.AddSingleton<ILinnApiActions>(
                i => new LinnApiActions(new RestClient(10000)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler(ExceptionHandlers.Handlers);
            app.UseBotwin();
        }
    }
}