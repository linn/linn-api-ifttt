namespace Linn.Api.Ifttt.Resources.Ifttt
{
    public class ErrorResource
    {
        public ErrorResource()
        {
        }

        public ErrorResource(string message, bool shouldSkip)
        {
            this.Errors = new[] { new ErrorMessage(message, shouldSkip ? "SKIP" : string.Empty) };
        }

        public ErrorMessage[] Errors { get; set; }
    }
}
