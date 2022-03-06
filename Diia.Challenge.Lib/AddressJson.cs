using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Diia.Challenge.Lib
{
    public class AddressJson
    {
        public List<CityJson> Cities { get; set; }
        public List<DistrictJson> CityDistricts { get; set; }
        public List<StreetJson> Streets { get; set; }
    }
}
