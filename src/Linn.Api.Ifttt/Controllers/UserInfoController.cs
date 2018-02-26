namespace Linn.Api.Ifttt.Controllers
{
    using Linn.Api.Ifttt.Resources;

    using Microsoft.AspNetCore.Mvc;

    [Route("ifttt/v1/user/info")]
    public class UserInfoController : Controller
    {
        [HttpGet]
        public IftttDataResource<UserInfoResource> Get()
        {
            var userInfo = new UserInfoResource("A.N. User", "/sub/userid");
            return new IftttDataResource<UserInfoResource>(userInfo);
        }
    }
}
