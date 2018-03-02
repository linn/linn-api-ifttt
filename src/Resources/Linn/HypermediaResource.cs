namespace Linn.Api.Ifttt.Resources.Linn
{
    using System.Collections.Generic;

    public abstract class HypermediaResource
    {
        protected HypermediaResource()
        {
            this.Links = new List<LinkResource>();
        }

        public IList<LinkResource> Links { get; set; }
    }
}