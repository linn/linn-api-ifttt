﻿namespace Linn.Api.Ifttt.Service
{
    using System.Linq;
    using System.Net;

    using Carter.Response;

    using Linn.Api.Ifttt.Proxies;
    using Linn.Api.Ifttt.Resources.Ifttt;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;

    public class ExceptionHandlers
    {
        public static void Handlers(IApplicationBuilder options)
        {
            options.Run(
                async context =>
                {
                    var ex = context.Features.Get<IExceptionHandlerFeature>();
                    switch (ex.Error)
                    {
                        case InvalidServiceKeyException _:
                            context.Response.StatusCode = 401;
                            break;
                        case LinnApiException linnApiException:
                            switch (linnApiException.StatusCode)
                            {
                                case HttpStatusCode.Forbidden:
                                case HttpStatusCode.Unauthorized:
                                    context.Response.StatusCode = 401;
                                    await context.Response.AsJson(new ErrorResource(linnApiException.Message, false), context.RequestAborted);
                                    break;
                                case HttpStatusCode.NotFound:
                                case HttpStatusCode.BadRequest:
                                    context.Response.StatusCode = 400;
                                    await context.Response.AsJson(new ErrorResource(linnApiException.Message, true), context.RequestAborted);
                                    break;
                                default:
                                    context.Response.StatusCode = (int)linnApiException.StatusCode;
                                    await context.Response.AsJson(new ErrorResource(linnApiException.Message, false), context.RequestAborted);
                                    break;
                            }

                            break;
                        case FluentValidation.ValidationException validationException:
                            var errorMessages = validationException.Errors.Select(m => new ErrorMessage(m.ErrorMessage, string.Empty)).ToArray();

                            context.Response.StatusCode = 400;
                            await context.Response.AsJson(new ErrorResource { Errors = errorMessages });

                            break;
                        default:
                            context.Response.StatusCode = 500;
                            await context.Response.AsJson(new ErrorResource(ex.Error.Message, false), context.RequestAborted);

                            break;
                    }
                });
        }
    }
}
