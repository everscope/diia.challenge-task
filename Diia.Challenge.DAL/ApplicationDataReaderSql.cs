//using Diia.Challenge.DatabaseContext;

using Diia.Challenge.DAL;
using Diia.Challenge.Lib;
using Diia.Challenge;

namespace Diia.Challenge
{
    public class ApplicationDataReaderSql : ApplicationDataReader
    {
        //private ApplicationContext _context;

        public ApplicationDataReaderSql(ApplicationContext context)
        {
            _context = context;
        }

        public override void GetApplicationData()
        {

        }

        public override void AddApplication(Application application)
        {
            _context.Applications.Add(application);
            _context.SaveChanges();
        }

        public override void UpdateApplicationStatus()
        {

        }

    }
}
