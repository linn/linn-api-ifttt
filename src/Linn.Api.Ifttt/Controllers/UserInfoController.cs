namespace Linn.Api.Ifttt.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Api.Ifttt.Resources;

    using Microsoft.AspNetCore.Mvc;

    [Route("ifttt/v1/user/info")]
    public class UserInfoController : Controller
    {
        private readonly IUserResourceFactory userResourceFactory;

        public UserInfoController(IUserResourceFactory userResourceFactory)
        {
            this.userResourceFactory = userResourceFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var accessToken = this.GetAccessToken();

            if (accessToken == null)
            {
                return this.Unauthorized();
            }

            return this.Ok(new IftttDataResource<UserInfoResource>(await this.userResourceFactory.Create(accessToken)));
        }

        private string GetAccessToken()
        {
            var authorizationHeader =
                this.Request.Headers["Authorization"].FirstOrDefault(h => h.StartsWith("Bearer "));

            return authorizationHeader?.Substring(7);
        }
    }
}
