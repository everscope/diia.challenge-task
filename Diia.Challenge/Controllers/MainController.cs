using Diia.Challenge.DAL;
using Diia.Challenge.Lib;
using Microsoft.AspNetCore.Mvc;

namespace Diia.Challenge.Controllers
{
    [ApiController]
    [Route("api/")]
    public class MainController : ControllerBase
    {
        private readonly ILogger<MainController> _logger;
        private readonly ApplicationDataReader _dataReader;
        private readonly ConfigurationDataReaderJson _configurationReader;
        private AddressValidator _addressValidator;

        public MainController(ILogger<MainController> logger,
                                ApplicationDataReader dataReader,
                                ConfigurationDataReaderJson configurationReader,
                                AddressValidator addressValidator)
        {
            _addressValidator = addressValidator;
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

            if (!_configurationReader.CheckAddress(address)
                && _addressValidator.CheckOnApplicationAdded(address))
            {
                _configurationReader.AddAddress(address);
            }

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
            _addressValidator.UnvalidateOnConfigurationChange();
            _configurationReader.SetThreshold(threshold);
        }

        [HttpPost("weights")]
        public void SetWeights(Weights weights)
        {
            _addressValidator.UnvalidateOnConfigurationChange();
            _configurationReader.SetWeights(weights);
        }
    }
}
