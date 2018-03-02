namespace Linn.Api.Ifttt.Resources.Ifttt
{
    using System.Collections.Generic;

    public class ActionRequestResource
    {
        public IDictionary<string, string> ActionFields { get; set; }

        public IDictionary<string, string> Ifttt_Source { get; set; }

        public IDictionary<string, string> User { get; set; }
    }
}
