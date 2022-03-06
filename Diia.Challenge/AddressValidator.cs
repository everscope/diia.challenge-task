using Diia.Challenge.DAL;
using Diia.Challenge.Lib;

namespace Diia.Challenge
{
    public class AddressValidator
    {
        private ApplicationContext _context;
        private ConfigurationDataReaderJson _configurationReader;
        public Dictionary<string, int> Weights { get; set; }
        public int Threshold { get; set; }

        public AddressValidator(ApplicationContext context,
            ConfigurationDataReaderJson configurationReader)
        {
            _configurationReader = configurationReader;
            _context = context;
        }

        public void UnvalidateOnConfigurationChange()
        {
            _configurationReader.RemoveAddresses(_context.Addresses.ToList());
        }

        public void CheckOnApplicationAdded(Address address)
        {

            if (Threshold == null)
            {
                Threshold = _configurationReader.GetThreshold().value;
            }

            if (Weights == null)
            {
                Weights = _configurationReader.GetWeights().weigths;
            }

            var previousApplications = _context.Applications.Where(p => p.City == address.CityId
                                                            && p.District == address.CityDistrictId
                                                            && p.Street == address.StreetId);

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
                _context.Addresses.Add(new AddressForSql()
                {
                    CityDistrictId = address.CityDistrictId,
                    CityId = address.CityId,
                    StreetId = address.StreetId
                });
                _context.SaveChanges();
            }
            
        }
    }
}
