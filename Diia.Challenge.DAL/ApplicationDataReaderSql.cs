using Diia.Challenge.DAL;
using Diia.Challenge.Lib;
using Diia.Challenge;

namespace Diia.Challenge
{
    public class ApplicationDataReaderSql : ApplicationDataReader
    {
        public ApplicationDataReaderSql(ApplicationContext context)
        {
            _context = context;
        }

        public override void AddApplication(Application application)
        {
            _context.Applications.Add(application);
            _context.SaveChanges();
        }

        public override List<AddressForSql> GetAllAddresses()
        {
            return _context.Addresses.ToList();
        }

        public override List<Application> GetApplicationWithAddress(Address address)
        {
            return _context.Applications.Where(p => p.City == address.CityId
                && p.District == address.CityDistrictId
                && p.Street == address.StreetId).ToList();
        }

        public override void AddAdress(AddressForSql adress)
        {
            _context.Addresses.Add(adress);
            _context.SaveChanges();
        }

        public override void UpdateApplicationStatus(string Id, string status)
        {
            var application = _context.Applications.FirstOrDefault(p => p.Id == Id);
            if (application != null)
            {
                application.Status = status;
                _context.Applications.Update(application);
                _context.SaveChanges();
            }
        }

    }
}
