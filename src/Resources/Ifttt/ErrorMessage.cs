﻿namespace Linn.Api.Ifttt.Resources.Ifttt
{
    public class ErrorMessage
    {
        public ErrorMessage(string message)
        {
            this.Message = message;
        }

        public string Message { get; }
    }
}