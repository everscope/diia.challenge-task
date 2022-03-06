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

        //public ApplicationDataReader(ApplicationContext context)
        //{
        //    _context = context;
        //}

        public abstract void AddApplication(Application application);
        public abstract void UpdateApplicationStatus(string Id, string status);

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

        //public Application CreateApplication(Address address)
        //{
        //    Application application = new Application();
        //    application.City = address.CityId;
        //    application.District = address.CityDistrictId;
        //    application.Street = address.StreetId;
        //    application.Status = "-1";
        //    application.Id = GenerateId();
        //}
    }
}
