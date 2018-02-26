namespace Linn.Api.Ifttt.Resources
{
    public class UserInfoResource
    {
        public UserInfoResource(string name, string id, string url = null)
        {
            this.Name = name;
            this.Id = id;
            this.Url = url;
        }

        public string Name { get; }

        public string Id { get; }

        public string Url { get; }
    }
}
