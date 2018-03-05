namespace Linn.Api.Ifttt.Proxies
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ILinnApiActions
    {
        Task<string> TurnOffAllDevices(string accessToken, CancellationToken ct);

        Task<string> TurnOffDevice(string accessToken, string deviceId, CancellationToken ct);

        Task<IDictionary<string, string>> FindAllDevices(string accessToken, CancellationToken ct);
    }
}