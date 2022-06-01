using Diia.Challenge.DAL;
using Diia.Challenge.Lib;
using AutoMapper;

namespace Diia.Challenge
{
    public class AddressValidator
    {
        private ApplicationDataReader _dataReader;
        private IConfigurationDataReader _configurationReader;
        private IMapper _mapper;
        public Dictionary<string, int> Weights { get; set; }
        public int? Threshold { get; set; } = null;

        public AddressValidator(ApplicationDataReader dataReader,
            IConfigurationDataReader configurationReader,
            IMapper mapper)
        {
            _configurationReader = configurationReader;
            _dataReader = dataReader;
            _mapper = mapper;
        }

        public void UnvalidateOnConfigurationChange()
        {
            _configurationReader.RemoveAddresses(_dataReader.GetAllAddresses()); ;
        }

        public bool CheckOnApplicationAdded(Address address)
        {

            if (Threshold == null)
            {
                Threshold = _configurationReader.GetThreshold().value;
            }

            if (Weights == null)
            {
                Weights = _configurationReader.GetWeights().weigths;
            }

            var previousApplications = _dataReader.GetApplicationWithAddress(address);

            int x = 1;
            foreach (Application application in previousApplications)
            {
                if (application.Status != "-1")
                {
                    x *= Weights[application.Status];
                }
            }

            if (x > Threshold)
            { 
                _dataReader.AddAdress(_mapper.Map<AddressForSql>(address));
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
