namespace Linn.Api.Ifttt.Resources.Linn
{
    using System;

    public class LinkResource
    {
        public LinkResource()
        {
        }

        public LinkResource(string rel, string href)
        {
            this.Rel = rel;
            this.Href = href;
        }

        public LinkResource(string rel, Uri uri)
        {
            this.Rel = rel;
            this.Href = uri.ToString();
        }

        public string Href { get; set; }

        public string Rel { get; set; }
    }
}