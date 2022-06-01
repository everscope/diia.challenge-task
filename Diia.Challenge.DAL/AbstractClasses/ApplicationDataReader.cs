using Diia.Challenge.DAL;
using Diia.Challenge.Lib;

namespace Diia.Challenge
{
    public abstract class ApplicationDataReader
    {
        private readonly int _idLength = 12;
        private readonly char[] _charsForId =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890".ToCharArray();

        protected ApplicationContext _context;

        public abstract void AddApplication(Application application);
        public abstract void UpdateApplicationStatus(string Id, string status);
        public abstract List<Application> GetApplicationWithAddress(Address address);
        public abstract void AddAdress(AddressForSql adress);
        public abstract List<AddressForSql> GetAllAddresses();

        public string GenerateId()
        {
            string result = String.Empty;

            Random random = new Random();
            for (int i = 0; i < _idLength; i++)
            {
                result += _charsForId[random.Next(0, _charsForId.Length - 1)];
            }

            if (_context.Applications.Any(p => p.Id == result))
            {
                return result = GenerateId();
            }
            else
            {
                return result;
            }
        }
    }
}
