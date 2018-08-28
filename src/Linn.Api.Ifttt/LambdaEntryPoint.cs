namespace Linn.Api.Ifttt
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;

    public class LambdaEntryPoint : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder
                .UseStartup<Startup>().ConfigureLogging((ctx, logging) => { logging.AddFilter(null, LogLevel.Error); });
        }
    }
}
