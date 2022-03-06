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
            var previousApplications = _context.Applications.Where(p => p.City == address.CityId
                                                            && p.District == address.CityDistrictId
                                                            && p.Street == address.StreetId);

            int x = 1;
            foreach (Application application in previousApplications)
            {
                x *= Weights[application.Status];
            }

            if (x > Threshold)
            {
                _context.Addresses.Add(address);
                _context.SaveChanges();
            }
            
        }
    }
}
