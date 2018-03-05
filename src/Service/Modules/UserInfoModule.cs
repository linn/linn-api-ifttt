namespace Linn.Api.Ifttt.Service.Modules
{
    using System.Threading.Tasks;

    using Botwin;
    using Botwin.Response;

    using Linn.Api.Ifttt.Resources.Ifttt;
    using Linn.Api.Ifttt.Service.Factories;

    public class UserInfoModule : BotwinModule
    {
        public UserInfoModule(IUserResourceFactory userResourceFactory)
        {
            this.RequiresAccessToken();

            this.Get(
                "/ifttt/v1/user/info",
                async (req, res, routeData) =>
                    {
                        var userInfoResource = await userResourceFactory.Create(req.GetAccessToken(), req.HttpContext.RequestAborted);

                        if (userInfoResource == null)
                        {
                            res.StatusCode = 401;
                        }
                        else
                        {
                            await res.AsJson(new DataResource<UserInfoResource>(userInfoResource), req.HttpContext.RequestAborted);
                        }
                    });
        }
    }
}
