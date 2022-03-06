using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Diia.Challenge.Lib;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Diia.Challenge.DAL
{
    public class ConfigurationDataReaderJson
    {
        public void SetThreshold(Threshold threshold)
        {
            byte [] jsonBytes = JsonSerializer.SerializeToUtf8Bytes<Threshold>(threshold);
            File.WriteAllBytes("..\\Diia.Challenge.DAL\\Data\\threshhold.json", jsonBytes);
        }

        public Threshold GetThreshold()
        {
            byte[] jsonBytes = File.ReadAllBytes("..\\Diia.Challenge.DAL\\Data\\threshhold.json");
            var readOnlyTemp = new ReadOnlySpan<byte>(jsonBytes);
            return JsonSerializer.Deserialize<Threshold>(readOnlyTemp);
        }

        public void SetWeights(Weights weights)
        {
            byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes<Weights>(weights);
            File.WriteAllBytes("..\\Diia.Challenge.DAL\\Data\\weights.json", jsonBytes);
        }

        public Weights GetWeights()
        {
            byte[] jsonBytes = File.ReadAllBytes("..\\Diia.Challenge.DAL\\Data\\weights.json");
            var readOnlyTemp = new ReadOnlySpan<byte>(jsonBytes);
            return JsonSerializer.Deserialize<Weights>(readOnlyTemp);
        }

        public void AddAddress(Address address)
        {
            byte[] jsonBytesToRead = File.ReadAllBytes("..\\Diia.Challenge.DAL\\Data\\addresses.json");
            var readOnlyTemp = new ReadOnlySpan<byte>(jsonBytesToRead);
            Addresses addresses = JsonSerializer.Deserialize<Addresses>(readOnlyTemp);

            addresses.addresses.Add(address);

            byte[] jsonBytesToWrite = JsonSerializer.SerializeToUtf8Bytes<Addresses>(addresses);
            File.WriteAllBytes("..\\Diia.Challenge.DAL\\Data\\addresses.json", jsonBytesToWrite);

        }

        public Addresses GetAddresses()
        {
            byte[] jsonBytesToRead = File.ReadAllBytes("..\\Diia.Challenge.DAL\\Data\\addresses.json");
            var readOnlyTemp = new ReadOnlySpan<byte>(jsonBytesToRead);
            return JsonSerializer.Deserialize<Addresses>(readOnlyTemp);
        }
    }
}
