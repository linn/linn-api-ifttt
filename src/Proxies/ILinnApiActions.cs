namespace Linn.Api.Ifttt.Proxies
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ILinnApiActions
    {
        Task<string> TurnOfAllDevices(string accessToken, CancellationToken ct);
    }
}