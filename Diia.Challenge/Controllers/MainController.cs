using AutoMapper;
using Diia.Challenge.DAL;
using Diia.Challenge.Lib;
using Microsoft.AspNetCore.Mvc;

namespace Diia.Challenge.Controllers
{
    [ApiController]
    [Route("api/")]
    public class MainController : ControllerBase
    {
        private readonly ApplicationDataReader _dataReader;
        private readonly IConfigurationDataReader _configurationReader;
        private AddressValidator _addressValidator;
        private IMapper _mapper;

        public MainController(ApplicationDataReader dataReader,
            IConfigurationDataReader configurationReader,
            AddressValidator addressValidator,
            IMapper mapper)
        {
            _addressValidator = addressValidator;
            _configurationReader = configurationReader;
            _dataReader = dataReader;
            _mapper = mapper;
        }

        [HttpPost("application")]
        public string AddApplication(Address address)
        {
            Application application = _mapper.Map<Application>(address);
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
