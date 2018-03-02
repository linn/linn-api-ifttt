namespace Linn.Api.Ifttt.Testing.Unit.LinnApiActions
{
    using Linn.Api.Ifttt.Proxies;
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
    }
}
