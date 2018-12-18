namespace Linn.Api.Ifttt.Proxies
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ILinnApiActions
    {
        Task<string> TurnOffAllDevices(string accessToken, CancellationToken ct);

        Task<string> TurnOffDevice(string accessToken, string deviceId, CancellationToken ct);

        Task<IDictionary<string, string>> GetDeviceNames(string accessToken, CancellationToken ct);

        Task<string> PlaySingleMedia(string accessToken, string deviceId, string mediaUri, string mediaTitle, string mediaArtworkUrl, CancellationToken ct);

        Task<string> PlayPlaylist(string accessToken, string deviceId, string playlistId, CancellationToken ct);

        Task<string> MuteDevice(string accessToken, string deviceId, CancellationToken ct);

        Task<string> UnmuteDevice(string accessToken, string deviceId, CancellationToken ct);

        Task<string> InvokePin(string accessToken, string deviceId, string pinId, CancellationToken ct);

        Task<IDictionary<string, string>> GetPlaylistNames(string accessToken, CancellationToken ct);

        Task<IDictionary<string, string>> GetDeviceSourceNames(string accessToken, string deviceId, CancellationToken ct);

        Task<string> SelectSource(string accessToken, string deviceId, string sourceId, CancellationToken ct);
    }
}