namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    using System;
    using System.Net.Http;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Api.Ifttt.Service.Factories;
    using Linn.Api.Ifttt.Service.Modules;
    using Linn.Common.Configuration;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    public class ContextBase
    {
        public ContextBase()
        {
            this.AccessToken = Guid.NewGuid().ToString();

            this.IftttServiceKey = "VALID_SERVICE_KEY";

            ConfigurationManager.Configuration["iftttServiceKey"] = this.IftttServiceKey;

            this.UserInfoResourceFactory = Substitute.For<IUserResourceFactory>();

            this.LinnApiActions = Substitute.For<ILinnApiActions>();

            this.Client = new TestClient()
                .WithAssembly(typeof(UserInfoModule).Assembly)
                .With(s =>
                    {
                        s.AddSingleton(this.UserInfoResourceFactory);
                        s.AddSingleton(this.LinnApiActions);
                    });
        }

        protected string IftttServiceKey { get; }

        protected IUserResourceFactory UserInfoResourceFactory { get; }

        protected ILinnApiActions LinnApiActions { get; }

        protected HttpClient Client { get; }

        protected string AccessToken { get; }
    }
}
