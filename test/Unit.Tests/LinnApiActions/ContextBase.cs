namespace Linn.Api.Ifttt.Testing.Unit.LinnApiActions
{
    using System;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Api.Ifttt.Resources.Linn;
    using Linn.Common.Configuration;
    using Linn.Common.Proxy;

    using NSubstitute;

    public class ContextBase
    {
        public ContextBase()
        {
            this.RestClient = Substitute.For<IRestClient>();
            ConfigurationManager.Configuration["apiRoot"] = "http://localhost";
            this.Sut = new LinnApiActions(this.RestClient);
        }

        protected IRestClient RestClient { get; }

        protected LinnApiActions Sut { get; }

        protected static PlayerResource GeneratePlayerResource()
        {
            var id = Guid.NewGuid().ToString();
            var sources = new SourceResource[] { GenerateSourceResource(), GenerateSourceResource() };
            var resource = new PlayerResource { Id = id, Sources = sources };
            resource.Links.Add(new LinkResource("standby", $"/{Guid.NewGuid().ToString()}"));
            return resource;
        }

        protected static SourceResource GenerateSourceResource()
        {
            var id = Guid.NewGuid().ToString();
            var resource = new SourceResource { Id = id, Visible = true };
            return resource;
        }

        protected static PlaylistResource GeneratePlaylistResource()
        {
            var id = Guid.NewGuid().ToString();
            var resource = new PlaylistResource { Id = id };
            return resource;
        }
    }
}
