using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using webapi.Domain;
using webapi.DTOs;
using webapi.Models;
using Xunit;

namespace webapi.Tests
{
    [Collection("Integration Tests")]
    public class EmailControllerIntegrationTests
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private Email _modelEmail = null;

        public EmailControllerIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;

            var dbContext = _factory.Services.GetRequiredService<EmailDbContext>();
            _modelEmail =  new Email()
            {
                Id = 1,
                Description = "Email Text",
                Priority = 4,
                Sender = "sender@gmail.com",
                Recipients = "recipient@gmail.com",
                Status = EmailStatus.Pending
            };
            dbContext.Emails.Add(_modelEmail);
            dbContext.SaveChanges();
        }

        // [Fact]
        // public async Task GetRoot_ReturnsSuccessAndStatusUp()
        // {
        //     // Arrange
        //     var client = _factory.CreateClient();

        //     // Act
        //     var response = await client.GetAsync("/");

        //     // Assert
        //     response.EnsureSuccessStatusCode();
        //     Assert.NotNull(response.Content);
        //     var responseObject = JsonSerializer.Deserialize<ResponseType>(
        //         await response.Content.ReadAsStringAsync(),
        //         new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
        //     Assert.Equal("Up", responseObject?.Status);
        // }




         [Fact]
        public async Task GetEmail_ReturnsSuccessAndEmail()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/email/get/1");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Content);
            var responseEmail = JsonSerializer.Deserialize<EmailDTO>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
            Assert.NotNull(responseEmail);
            Assert.NotNull(_modelEmail);
            Assert.Equal(_modelEmail.Id, responseEmail.Id);
            Assert.Equal(_modelEmail.Recipients, responseEmail.Recipients);
            Assert.Equal(_modelEmail.Description, responseEmail.Description);
            Assert.Equal(_modelEmail.Priority, responseEmail.Priority);
            Assert.Equal(_modelEmail.Recipients, responseEmail.Recipients);
        }
    }
}
