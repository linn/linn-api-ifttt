# IFTTT to Linn API Shim

This project is a ASP.NET Core Web API project run in AWS Lambda exposed through Amazon API Gateway. 

The NuGet package [Amazon.Lambda.AspNetCoreServer](https://www.nuget.org/packages/Amazon.Lambda.AspNetCoreServer) contains a Lambda function that is used to translate requests from API Gateway into the ASP.NET Core framework and then the responses from ASP.NET Core back to API Gateway.

From there, Carter is used as a shim over ASP.NET to provide Nancy style routing, binding and negotiation. 

## Test

```
    dotnet test test/Integration.Tests/
    dotnet test test/Unit.Tests/
```

## Deploy

Use Docker to build a deployment container:

```bash
docker build -t deploy .
```

Put deployment credentials in an `.env` file:

```
IFTTT_SERVICE_KEY=
IFTTT_TEST_CLIENT_ID=
IFTTT_TEST_CLIENT_SECRET=
IFTTT_TEST_USERNAME=
IFTTT_TEST_PASSWORD=
AWS_DEFAULT_REGION=...
AWS_ACCESS_KEY_ID=...
AWS_SECRET_ACCESS_KEY=...
```

and

```bash
docker run --env-file=.env deploy
```
