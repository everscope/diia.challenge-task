using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Diia.Challenge.Lib;

namespace Diia.Challenge.DAL
{
    public class ConfigurationDataReaderJson
    {
        public void SetThreshold(Threshold threshold)
        {
            byte [] jsonBytes = JsonSerializer.SerializeToUtf8Bytes<Threshold>(threshold);
            File.WriteAllBytes("..\\Diia.Challenge.DAL\\Data\\threshhold.json", jsonBytes);
        }

        public void SetWeights(Weights weights)
        {
            byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes<Weights>(weights);
            File.WriteAllBytes("..\\Diia.Challenge.DAL\\Data\\weights.json", jsonBytes);
        }

        public void AddAddress()
        {

        }
    }
}
