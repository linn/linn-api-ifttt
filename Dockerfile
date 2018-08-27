FROM microsoft/dotnet:2.1-sdk AS build
COPY . /repo
WORKDIR /repo
RUN dotnet test test/Integration.Tests/
RUN dotnet test test/Unit.Tests/
RUN apt-get update && apt-get install -y zip
WORKDIR /repo/src/Linn.Api.Ifttt
RUN dotnet lambda package --configuration release --framework netcoreapp2.1 --output-package bin/release/netcoreapp2.1/package.zip

FROM node:carbon-alpine
COPY --from=build /repo/src/Linn.Api.Ifttt/bin/release/netcoreapp2.1/package.zip /app/
COPY serverless.yml /app/
WORKDIR /app
RUN npm install -g serverless
ENTRYPOINT [ "serverless" ]
CMD [ "deploy", "--force" ]