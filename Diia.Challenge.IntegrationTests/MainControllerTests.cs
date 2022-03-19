using System.Security.Cryptography.X509Certificates;
using Castle.Core.Logging;
using Diia.Challenge.Controllers;
using Diia.Challenge.DAL;
using Diia.Challenge.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;

namespace Diia.Challenge.IntegrationTests
{
    public class MainControllerTests
    {
        //[Fact]
        //public void AddApplication_ShouldAddNewApplicationToDatabaseThatAlreadyExistsWithoudAddingToDictionary()
        //{
        //    var context = new Mock<ApplicationContext>();
        //    var configurationDataReaderJsonMoq = new Mock<ConfigurationDataReaderJson>();
        //    var addressValidatorMoq = new Mock<AddressValidator>();
        //    var applicationContextReader = new Mock<ApplicationDataReaderSql>(context);

        //    //configurationDataReaderJsonMoq.Setup(p => p.CheckAddress(It.IsAny<Address>())).Returns(true);
        //    //addressValidatorMoq.Setup(p => p.CheckOnApplicationAdded(It.IsAny<Address>())); 
        //    //applicationContextReader.Setup(p => p.AddApplication(It.IsAny<Application>()));

        //    configurationDataReaderJsonMoq.Setup(moq => moq.CheckAddress(It.IsAny<Address>()));

        //    MainController controller = new(applicationContextReader.Object, 
        //                                    configurationDataReaderJsonMoq.Object, 
        //                                    addressValidatorMoq.Object);

        //    Address address = new Address()
        //    {
        //        CityDistrictId = "Holosiiv",
        //        CityId = "Kyiv",
        //        StreetId = "Hlushkova"
        //    };

        //    string res = controller.AddApplication(address);

        //    Assert.IsType<string>(res);
        //    Assert.NotNull(res);

        //    applicationContextReader.Verify(p => p.AddApplication(new Application()), Times.Once);
        //    addressValidatorMoq.Verify(p => p.CheckOnApplicationAdded(new Address()), Times.Once);
        //}
    }
}