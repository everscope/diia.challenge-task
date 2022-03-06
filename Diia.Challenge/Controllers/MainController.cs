using System.Globalization;
using System.Net;
using Diia.Challenge.DAL;
using Diia.Challenge.Lib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Diia.Challenge.Controllers
{
    [ApiController]
    [Route("api/")]
    public class MainController : ControllerBase
    {
        private readonly ILogger<MainController> _logger;
        private readonly ApplicationDataReader _dataReader;
        private readonly ConfigurationDataReaderJson _configurationReader;

        public MainController(ILogger<MainController> logger,
                                ApplicationDataReader dataReader,
                                ConfigurationDataReaderJson configurationReader)
        {
            _configurationReader = configurationReader;
            _dataReader = dataReader;
            _logger = logger;
        }

        [HttpPost("application")]
        public string AddApplication(Address address)
        {
            Application application = new Application();

            application.City = address.CityId;
            application.District = address.CityDistrictId;
            application.Street = address.StreetId;
            application.Status = "-1";
            application.Id = _dataReader.GenerateId();

            _dataReader.AddApplication(application);

            return application.Id;
        }

        [HttpPost("application/{id}")]
        public void ChangeAplicationStatus([FromRoute] string id, Status status)
        {
            _dataReader.UpdateApplicationStatus(id, status.status);
        }

        [HttpPost("threshold")]
        public void SetTreshhold(Threshold threshold)
        {
            _configurationReader.SetThreshold(threshold);
        }

        [HttpPost("weights")]
        public void SetWeights(Weights weights)
        {
            _configurationReader.SetWeights(weights);
        }
    }
}
