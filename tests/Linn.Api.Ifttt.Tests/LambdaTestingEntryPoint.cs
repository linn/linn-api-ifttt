namespace Linn.Api.Ifttt.Tests
{
    using Linn.Api.Ifttt.Controllers;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public class LambdaTestingEntryPoint : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {
        private readonly IUserResourceFactory userResourceFactory;

        public LambdaTestingEntryPoint(IUserResourceFactory userResourceFactory)
        {
            this.userResourceFactory = userResourceFactory;
        }

        protected override void Init(IWebHostBuilder builder)
        {
            builder.UseStartup<TestingStartup>().ConfigureServices(this.ConfigureServices);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(i => this.userResourceFactory);
        }
    }
}
