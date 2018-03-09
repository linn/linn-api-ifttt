namespace Linn.Api.Ifttt.Proxies
{
    using System;
    using System.Net;

    public class LinnApiException : Exception
    {
        public LinnApiException(HttpStatusCode statusCode) : base($"Linn API status code: {statusCode}")
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }

    }
}
