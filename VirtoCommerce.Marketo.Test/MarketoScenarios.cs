﻿using System;
using System.Security.Authentication;
using VirtoCommerce.Marketo.Data.Models;
using VirtoCommerce.Marketo.Data.Services;
using Xunit;

namespace VirtoCommerce.Marketo.Test
{
    public class MarketoScenarios
    {
        [Fact]
        [Trait("Category", "Unit")]
        public void Can_create_leads()
        {
            var serviceName = Environment.GetEnvironmentVariable("MarketoUrl");
            var connection = new MarketoConnectionInfo() {
                RestApiUrl = Environment.GetEnvironmentVariable("MarketoUrl"),
                ClientId = Environment.GetEnvironmentVariable("MarketoClientId"),
                ClientSecret = Environment.GetEnvironmentVariable("MarketoClientSecret")
            };

            var service = new MarketoService(connection);

            var request = new LeadsRequest
            {
                lookupField = "email",
                input = new[] { new Lead { postalCode = "90069", email = "test@user.com", firstName = "John", lastName = "Doe", leadScore = 10, address = "sample address", city = "los angeles", country = "United States", phone = "234234234", state = "CA" } }
            };

            var result = service.CreateOrUpdateLeads(request).Result;

            Assert.True(result.success);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Can_create_leads_unauthorized()
        {
            var serviceName = Environment.GetEnvironmentVariable("MarketoUrl");
            var connection = new MarketoConnectionInfo()
            {
                RestApiUrl = Environment.GetEnvironmentVariable("MarketoUrl"),
                ClientId = "fake",
                ClientSecret = "fakke"
            };

            var service = new MarketoService(connection);

            var request = new LeadsRequest
            {
                lookupField = "email",
                input = new[] { new Lead { postalCode = "90069", email = "test@user.com", firstName = "John", lastName = "Doe", leadScore = 10, address = "sample address", city = "los angeles", country = "United States", phone = "234234234", state = "CA" } }
            };

            var exception = Record.Exception(() => service.CreateOrUpdateLeads(request).Result);

            Assert.IsType(typeof(AuthenticationException), exception.InnerException.InnerException);
        }
    }
}
