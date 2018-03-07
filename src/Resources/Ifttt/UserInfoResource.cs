namespace Linn.Api.Ifttt.Resources.Ifttt
{
    public class UserInfoResource
    {
        public UserInfoResource(string name, string id)
        {
            this.Name = name;
            this.Id = id;
        }

        public string Name { get; }

        public string Id { get; }
    }
}
