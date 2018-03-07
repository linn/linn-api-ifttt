namespace Linn.Api.Ifttt.Service.Modules
{
    using System;

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
                        try
                        {
                            var userInfoResource = await userResourceFactory.Create(req.GetAccessToken(), req.HttpContext.RequestAborted);

                            await res.AsJson(new DataResource<UserInfoResource>(userInfoResource), req.HttpContext.RequestAborted);
                        }
                        catch (Exception e) 
                        {
                            res.StatusCode = 401;

                            await res.AsJson(new ErrorResource(e.Message), req.HttpContext.RequestAborted);
                        }
                    });
        }
    }
}
