namespace Linn.Api.Ifttt.Testing.Unit.LinnApiActions
{
    using System;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Api.Ifttt.Resources.Linn;
    using Linn.Common.Proxy;

    using NSubstitute;

    public class ContextBase
    {
        public ContextBase()
        {
            this.RestClient = Substitute.For<IRestClient>();
            this.Sut = new LinnApiActions(this.RestClient, "http://localhost");
        }

        protected IRestClient RestClient { get; }

        protected LinnApiActions Sut { get; }

        protected static PlayerResource GeneratePlayerResource()
        {
            var id = Guid.NewGuid().ToString();
            var playerResource = new PlayerResource { Id = id };
            playerResource.Links.Add(new LinkResource("standby", $"/{Guid.NewGuid().ToString()}"));
            return playerResource;
        }
    }
}
