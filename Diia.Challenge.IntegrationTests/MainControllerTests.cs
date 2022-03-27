using System.Net.Http.Json;
using System.Text.Json;
using Diia.Challenge.DAL;
using Diia.Challenge.Lib;
using Microsoft.EntityFrameworkCore;
using Xunit;


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

        [Fact]
        public async Task AddApplication_AddingNotExistingApplications_ShouldSaveThemToJson()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/threshold");
            request.Content = JsonContent.Create(new Threshold() {value = 5});
            var responce = await _client.SendAsync(request);
            responce.EnsureSuccessStatusCode();

            request = new HttpRequestMessage(HttpMethod.Post, "api/weights");
            request.Content = JsonContent.Create(new Weights()
            {
                weigths = new Dictionary<string, int>()
                {
                    {"success", 2},
                    {"good", 1}
                }
            });
            responce = await _client.SendAsync(request);
            responce.EnsureSuccessStatusCode();

            request = new HttpRequestMessage(HttpMethod.Post, "api/application");
            request.Content = JsonContent.Create(new Address()
            {
                CityId = "Odessa",
                CityDistrictId = "Sea",
                StreetId = "First"
            });
            responce = await _client.SendAsync(request);
            responce.EnsureSuccessStatusCode();

            string id = await responce.Content.ReadAsStringAsync();
            request = new HttpRequestMessage(HttpMethod.Post, $"api/application/{id}");
            request.Content = JsonContent.Create(new Status() { status = "Success" });
            responce = await _client.SendAsync(request);
            responce.EnsureSuccessStatusCode();

            request = new HttpRequestMessage(HttpMethod.Post, "api/application");
            request.Content = JsonContent.Create(new Address()
            {
                CityId = "Odessa",
                CityDistrictId = "Sea",
                StreetId = "First"
            });
            responce = await _client.SendAsync(request);
            responce.EnsureSuccessStatusCode();

            var addressesJson = ReadDataFromJson<AddressJson>("JsonTestData\\addresses.json");
            var address = addressesJson.Streets.FirstOrDefault(p => p.parentId == "Odessa"
                                                        && p.cityDistrictId == "Sea"
                                                        && p.id == "First");
            Assert.NotNull(address);
        }
        
        [Fact]
        public async Task ChangeApplicationStatus_ShouldChangeData()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/application/testIdNum1");
            var requestModel = new Status() { status = "testStatus" };

            request.Content = JsonContent.Create(requestModel);

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            Assert.Equal("testStatus", _context?.Applications?.FirstOrDefault(p => p.Id == "testIdNum1")?.Status);
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
                        "check up required", 4
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