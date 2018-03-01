namespace Linn.Api.Ifttt.Service.Factories
{
    using System.Threading.Tasks;

    using Linn.Api.Ifttt.Resources;

    public interface IUserResourceFactory
    {
        Task<UserInfoResource> Create(string accessToken);
    }
}