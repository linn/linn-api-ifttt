namespace Linn.Api.Ifttt.Resources.Ifttt
{
    public class SelectSourceActionFieldResource : ActionFieldResource
    {
        public class DeviceSourceResource
        {
            public string DeviceId { get; set; }
            public string SourceId { get; set; }
        }

        public string DeviceSource_Id { get; set; }
    }
}
