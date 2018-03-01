namespace Linn.Api.Ifttt
{
    using Botwin;

    using Linn.Api.Ifttt.Service.Factories;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBotwin();

            services.AddSingleton<IUserResourceFactory>(
                i => new UserResourceFactory(this.Configuration["discoveryEndpoint"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app)
        {
            app.UseBotwin();
        }
    }
}