namespace Linn.Api.Ifttt.Resources.Ifttt
{
    public class ErrorMessage
    {
        public ErrorMessage(string message, string status)
        {
            this.Status = status;
            this.Message = message;
        }

        public string Status { get; }

        public string Message { get; }
    }
}