﻿using System;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using Linn.Api.Ifttt.Resources.Ifttt;
using Newtonsoft.Json;
using Xunit;

namespace Linn.Api.Ifttt.Testing.Integration.Modules
{
    public class WhenSelectSourceOnASpecificDeviceWithMissingSourceId : ContextBase
    {
        private readonly HttpResponseMessage response;

        private readonly ErrorResource result;

        public WhenSelectSourceOnASpecificDeviceWithMissingSourceId()
        {
            var deviceId = Guid.NewGuid().ToString();

            var request = new
            {
                actionFields = new { device_Id = deviceId },
                ifttt_source = new { id = "2", url = "https://ifttt.com/myrecipes/personal/2" },
                user = new { timezone = "Pacific Time (US & Canada)" }
            };

            var content = new StringContent(JsonConvert.SerializeObject(request));

            this.Client.SetAccessToken(this.AccessToken);

            this.response = this.Client.PostAsync("/ifttt/v1/actions/select_source", content).Result;

            this.result = this.response.JsonBody<ErrorResource>();
        }

        [Fact]
        public void ShouldReturnBadRequest()
        {
            this.response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void ShouldDescribeError()
        {
            this.result.Errors.Should().HaveCount(1);
            this.result.Errors[0].Message.Should().Be("Action field `source_id` missing");
        }
    }
}
