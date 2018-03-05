namespace Linn.Api.Ifttt
{
    using Botwin;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Api.Ifttt.Service.Factories;
    using Linn.Api.Ifttt.Service.Modules;
    using Linn.Common.Proxy;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
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
            app.UseBotwin();
        }
    }
}