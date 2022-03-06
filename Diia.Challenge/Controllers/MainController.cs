using System.Globalization;
using System.Net;
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

        public MainController(ILogger<MainController> logger, ApplicationDataReader dataReader)
        {
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
        public string SetTreshhold(int threshold)
        {
            return "threshold";
        }

        [HttpPost("weights")]
        public string SetWeights()
        {
            return "weights";
        }
    }
}
