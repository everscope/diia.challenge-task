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
