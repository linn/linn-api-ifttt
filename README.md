# IFTTT to Linn API Shim

This project is a ASP.NET Core Web API project run in AWS Lambda exposed through Amazon API Gateway. 

The NuGet package [Amazon.Lambda.AspNetCoreServer](https://www.nuget.org/packages/Amazon.Lambda.AspNetCoreServer) contains a Lambda function that is used to translate requests from API Gateway into the ASP.NET Core framework and then the responses from ASP.NET Core back to API Gateway.

From there, Botwin is used as a shim over ASP.NET to provide Nancy style routing, binding and negotiation. 

## Here are some steps to follow from Visual Studio:

To deploy your Serverless application, right click the project in Solution Explorer and select *Publish to AWS Lambda*.

To view your deployed application open the Stack View window by double-clicking the stack name shown beneath the AWS CloudFormation node in the AWS Explorer tree. The Stack View also displays the root URL to your published application.

## Here are some steps to follow to get started from the command line:

Once you have edited your template and code you can use the following command lines to deploy your application from the command line (these examples assume the project name is *Linn.Api.Ifttt*):

### Execute unit tests
```
    dotnet test test/Integration.Tests/
    dotnet test test/Unit.Tests/
```

### Deploy application
```
    cd "src/Linn.Api.Ifttt"
    dotnet lambda deploy-serverless -cfg ../../aws-lambda-tools-defaults.json -t ../../serverless.template
```