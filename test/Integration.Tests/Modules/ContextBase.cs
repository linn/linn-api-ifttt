namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    using System;
    using System.Net.Http;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Api.Ifttt.Service.Factories;
    using Linn.Api.Ifttt.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    public class ContextBase
    {
        public ContextBase()
        {
            this.AccessToken = Guid.NewGuid().ToString();

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

        protected IUserResourceFactory UserInfoResourceFactory { get; }

        protected ILinnApiActions LinnApiActions { get; }

        protected HttpClient Client { get; }

        protected string AccessToken { get; }
    }
}
