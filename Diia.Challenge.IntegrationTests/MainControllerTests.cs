using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Castle.Core.Logging;
using Diia.Challenge.Controllers;
using Diia.Challenge.DAL;
using Diia.Challenge.Lib;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;
using Xunit.Extensions;
using System.Reflection;

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
            string responseData = await response.Content.ReadAsStringAsync();
            var applicationInDb = _context.Applications.FirstOrDefault(p => p.Id == responseData);

            response.EnsureSuccessStatusCode();
            Assert.NotNull(applicationInDb);
            Assert.Equal(requestModel.CityDistrictId, applicationInDb.District);
            Assert.Equal(requestModel.CityId, applicationInDb.City);
            Assert.Equal(requestModel.StreetId, applicationInDb.Street);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(10)]
        public async void SetThreshhold_ShouldSetTreshhold(int threshhold)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/threshold");
            var requestModel = new Threshold()
            {
                value = threshhold
            };

            request.Content = JsonContent.Create(requestModel);

            var responce = await _client.SendAsync(request);

            responce.EnsureSuccessStatusCode();

            Threshold currentThreshold = ReadDataFromJson<Threshold>("JsonTestData\\threshhold.json");

            Assert.Equal(threshhold, currentThreshold.value);
        }

        [Theory, MemberData(nameof(DataForSetWeightsTest))]
        public async Task SetWeights_ShouldSetWeights(Dictionary<string, int> weightsInput)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/weights");
            var requestModel = new Weights()
            {
                weigths = weightsInput
            };

            request.Content = JsonContent.Create(requestModel);

            var responce = await _client.SendAsync(request);

            responce.EnsureSuccessStatusCode();

            Weights currentWeights = ReadDataFromJson<Weights>("JsonTestData\\weights.json");

            Assert.Equal(weightsInput, currentWeights.weigths);
        }

        public static IEnumerable<object []> DataForSetWeightsTest
        {
            get
            {
                return new List<object[]>()
                {
                    new object[] { new Dictionary<string, int>()
                    {
                        {
                            "good", 5
                        },
                        {
                            "bad", 2
                        },
                        {
                            "failed", 1
                        }
                    }},
                    new object[] { new Dictionary<string, int>()
                    {
                    {
                        "verified", 10
                    },
                    {
                        "in process", 7
                    },
                    {
                        "requiers test", 4
                    },
                    {
                        "fake address", 0
                    }
                    }}
                };
            }
        }
        
        private T ReadDataFromJson<T>(string path)
        {
            byte[] jsonBytes = File.ReadAllBytes(path);
            var readOnlyTemp = new ReadOnlySpan<byte>(jsonBytes);
            return JsonSerializer.Deserialize<T>(readOnlyTemp);
        }
    }
}