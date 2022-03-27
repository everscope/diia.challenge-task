using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Diia.Challenge.DAL;
using Diia.Challenge.Lib;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Diia.Challenge.IntegrationTests
{
    public class TestingWebApplicationFactory <TEntryPoint> : WebApplicationFactory<Program> 
                                            where TEntryPoint : Program
    {
        public ApplicationContext Context { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContext = services.SingleOrDefault(p => p.ServiceType
                                                              == typeof(DbContextOptions<ApplicationContext>));
                ClearUpJson();

                if (dbContext != null)
                {
                    services.Remove(dbContext);
                }

                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseInMemoryDatabase("ApplicationContextForTesting");
                });

                var configurationDataReader = services.SingleOrDefault(p => p.ServiceType
                                                                            == typeof(IConfigurationDataReader));

                if (configurationDataReader != null)
                {
                    services.Remove(configurationDataReader);
                }
                services.AddTransient<IConfigurationDataReader>(s => new ConfigurationDataReaderJson(
                    "JsonTestData\\threshhold.json",
                    "JsonTestData\\weights.json",
                    "JsonTestData\\addresses.json"));

                var serviceProvider = services.BuildServiceProvider();
                using (var scope = serviceProvider.CreateScope())
                {
                    using (var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>())
                    {
                        try
                        {
                            applicationContext.Database.EnsureDeleted();
                            applicationContext.Database.EnsureCreated();
                            applicationContext.Applications.AddRange(new List<Application>()
                            {
                                new Application()
                                {
                                    City = "Kyiv",
                                    District = "Holosiiv",
                                    Street = "Main",
                                    Id = "testIdNum1",
                                    Status = "-1"
                                },
                                new Application()
                                {
                                    City = "Lviv",
                                    District = "Main",
                                    Street = "Central",
                                    Id = "testIdNum2",
                                    Status = "-1"
                                },
                                new Application()
                                {
                                    City = "Kyiv",
                                    District = "Holosiiv",
                                    Street = "Main",
                                    Id = "testIdNum3",
                                    Status = "-1"
                                },
                                new Application()
                                {
                                    City = "Odessa",
                                    District = "Sea",
                                    Street = "First",
                                    Id = "overcomingThreshhold1",
                                    Status = "success"
                                },
                                new Application()
                                {
                                    City = "Odessa",
                                    District = "Sea",
                                    Street = "First",
                                    Id = "overcomingThreshhold2",
                                    Status = "success"
                                },
                                new Application()
                                {
                                    City = "Odessa",
                                    District = "Sea",
                                    Street = "First",
                                    Id = "overcomingThreshhold3",
                                    Status = "success"
                                },

                            });

                            applicationContext.Addresses.AddRange(new List<AddressForSql>()
                            {
                                new AddressForSql()
                                {
                                    CityDistrictId = "East",
                                    CityId = "Zhytomyr",
                                    StreetId = "Second",
                                    Id = 1
                                },
                                new AddressForSql()
                                {
                                    CityDistrictId = "West",
                                    CityId = "Kyiv",
                                    StreetId = "First",
                                    Id = 2
                                }
                            });

                            applicationContext.SaveChanges();

                            Context = applicationContext;
                        }
                        catch
                        {
                            throw;
                        }
                    }

                    var jsonReader = scope.ServiceProvider.GetRequiredService<IConfigurationDataReader>();
                    try
                    {
                        ClearUpJson();
                        jsonReader.AddAddress(new Address()
                        {
                            CityId = "Cherkasy",
                            CityDistrictId = "West",
                            StreetId = "Railway"
                        });
                        jsonReader.AddAddress(new Address()
                        {
                            CityDistrictId = "East",
                            CityId = "Zhytomyr",
                            StreetId = "Second"
                        });
                        jsonReader.AddAddress(new Address()
                        {
                            CityDistrictId = "West",
                            CityId = "Kyiv",
                            StreetId = "First"
                        });

                        jsonReader.SetThreshold(new Threshold() {value = 10});
                        jsonReader.SetWeights(new Weights()
                        {
                            weigths = new Dictionary<string, int>() { { "testWeight", 1 } }
                        });
                    }
                    catch
                    {
                        throw;
                    }
                }

            });
        }

        private void ClearUpJson()
        {
            byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes<AddressJson>(new AddressJson());
            File.WriteAllBytes("JsonTestData\\addresses.json", jsonBytes);

            jsonBytes = JsonSerializer.SerializeToUtf8Bytes<Threshold>(new Threshold());
            File.WriteAllBytes("JsonTestData\\threshhold.json", jsonBytes);

            jsonBytes = JsonSerializer.SerializeToUtf8Bytes<Weights>(new Weights());
            File.WriteAllBytes("JsonTestData\\weights.json", jsonBytes);
        }
    }
}
