using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diia.Challenge.Lib;

namespace Diia.Challenge.DAL
{
    public interface IConfigurationDataReader
    {
        public void SetThreshold(Threshold threshold);
        public Threshold GetThreshold();
        public void SetWeights(Weights weights);
        public Weights GetWeights();
        public void AddAddress(Address address);
        public AddressJson GetAddresses();
        public bool CheckAddress(Address address);
        public void RemoveAddresses(List<AddressForSql> addressesToDelete);
    }
}

