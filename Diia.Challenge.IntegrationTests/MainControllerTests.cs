using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Castle.Core.Logging;
using Diia.Challenge.Controllers;
using Diia.Challenge.DAL;
using Diia.Challenge.Lib;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;

namespace Diia.Challenge.IntegrationTests
{
    public class MainControllerTests : IClassFixture<TestingWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ApplicationContext _context;

        public MainControllerTests(TestingWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            DbContextOptionsBuilder<ApplicationContext> optionsBuilder =
                new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseInMemoryDatabase("ApplicationContextForTesting");
            _context = new ApplicationContext(optionsBuilder.Options);
        }

        [Fact]
        public async Task ChangeApplicationStatus_ShouldChangeData()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/application/testIdNum1");
            var requestModel = new Status() {status = "testStatus"};

            request.Content = JsonContent.Create(requestModel);

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.Equal("testStatus", _context?.Applications?.FirstOrDefault(p => p.Id == "testIdNum1")?.Status);
        }

        [Fact]
        public async Task AddApplication_AddingExistingAddress_ShouldAddApplicationToDataBase()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/application");
            var requestModel = new Address()
            {
                CityId = "Cherkasy",
                CityDistrictId = "West",
                StreetId = "Railway"
            };

            request.Content = JsonContent.Create(requestModel);

            var response = await _client.SendAsync(request);

            var applicationInDb = _context.Applications.FirstOrDefault(p => p.Id == response.Content.ToString());

            response.EnsureSuccessStatusCode();
            Assert.NotNull(applicationInDb);
            Assert.Equal(requestModel.CityDistrictId, applicationInDb.District);
            Assert.Equal(requestModel.CityId, applicationInDb.City);
            Assert.Equal(requestModel.StreetId, applicationInDb.Street);

        }
    }
}