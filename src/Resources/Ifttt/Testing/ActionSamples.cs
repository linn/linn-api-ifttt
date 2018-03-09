namespace Linn.Api.Ifttt.Resources.Ifttt.Testing
{
    using System.Collections.Generic;

    public class ActionSamples
    {
        public IDictionary<string, string> turn_off_all_devices { get; set; }

        public IDictionary<string, string> turn_off_device { get; set; }

        public IDictionary<string, string> play_single_media { get; set; }

        public IDictionary<string, string> play_playlist { get; set; }

        public Dictionary<string, string> mute_device { get; set; }

        public Dictionary<string, string> unmute_device { get; set; }
    }
}