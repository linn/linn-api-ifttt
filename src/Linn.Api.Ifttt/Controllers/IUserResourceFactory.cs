namespace Linn.Api.Ifttt.Controllers
{
    using System.Threading.Tasks;

    using Linn.Api.Ifttt.Resources;

    public interface IUserResourceFactory
    {
        Task<UserInfoResource> Create(string accessToken);
    }
}