namespace Linn.Api.Ifttt.Resources.Ifttt
{
    using System.Collections.Generic;

    public class ActionRequestResource<T> where T : DeviceActionFieldResource
    {
        public T ActionFields { get; set; }

        public IDictionary<string, string> Ifttt_Source { get; set; }

        public IDictionary<string, string> User { get; set; }
    }
}
