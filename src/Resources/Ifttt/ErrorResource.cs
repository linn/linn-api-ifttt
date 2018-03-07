namespace Linn.Api.Ifttt.Resources.Ifttt
{
    public class ErrorResource
    {
        public ErrorResource()
        {
        }

        public ErrorResource(string message)
        {
            this.Errors = new[] { new ErrorMessage(message) };
        }

        public ErrorMessage[] Errors { get; set; }
    }
}
