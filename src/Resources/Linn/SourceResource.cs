namespace Linn.Api.Ifttt.Resources.Linn
{
    public class SourceResource : HypermediaResource
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool Visible { get; set; }
    }
}
