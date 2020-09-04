using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using webapi.Controllers;
using webapi.Domain;
using webapi.DTOs;
using webapi.Models;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace webapi.Tests
{
    [Collection("Unit Tests")]
    public class EmailControllerUnitTests
    {
        private readonly ITestOutputHelper output;
        public EmailControllerUnitTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void GetItems_ReturnsSingleItem()
        {
            var builder = new DbContextOptionsBuilder<EmailDbContext>();
            builder.UseInMemoryDatabase<EmailDbContext>(Guid.NewGuid().ToString());
            var options = builder.Options;

            using (var context = new EmailDbContext(options))
            {
                var orders = new List<Email>
                {
                    new Email { Id = 33, Subject = "Email Subject"},
                };

                context.Emails.AddRange(orders);
                context.SaveChanges();
            }

            using (var context = new EmailDbContext(options))
            {
                var loggerMock = new Mock<ILogger<EmailController>>(); 
                var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapping>());
                var mapper = config.CreateMapper();
                var settingsMock = new Mock<IOptions<AppSettings>>();
                var controller = new EmailController(loggerMock.Object, mapper, context, settingsMock.Object);
                var result = controller.Get();

                var okResult = result as OkObjectResult;

                // assert
                Assert.NotNull(okResult);
                Assert.True(okResult is OkObjectResult);
                Assert.IsType<List<EmailDTO>>(okResult.Value);

                Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
                var collection = okResult.Value as List<EmailDTO>;
                Assert.Single(collection);
            }
        }
    }
}
