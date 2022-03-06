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
            AddressJson addresses = JsonSerializer.Deserialize<AddressJson>(readOnlyTemp);

            if (!addresses.Cities.Any(p => p.id == address.CityId))
            {
                addresses.Cities.Add(new CityJson() {id = address.CityId} );
            }
            
            if(!addresses.CityDistricts.Any(p => p.id == address.CityDistrictId 
                                        && p.parentId == address.CityId))
            {
                addresses.CityDistricts.Add(new DistrictJson()
                {
                    id = address.CityDistrictId,
                    parentId = address.CityId
                });
            }

            if (!addresses.Streets.Any(p => p.id == address.StreetId
                                            && p.parentId == address.CityId
                                            && p.cityDistrictId == address.CityDistrictId))
            {
                addresses.Streets.Add(new StreetJson()
                {
                    id = address.StreetId,
                    parentId = address.CityId,
                    cityDistrictId = address.CityDistrictId
                });
            }

            byte[] jsonBytesToWrite = JsonSerializer.SerializeToUtf8Bytes<AddressJson>(addresses);
            File.WriteAllBytes("..\\Diia.Challenge.DAL\\Data\\addresses.json", jsonBytesToWrite);

        }

        public AddressJson GetAddresses()
        {
            byte[] jsonBytesToRead = File.ReadAllBytes("..\\Diia.Challenge.DAL\\Data\\addresses.json");
            var readOnlyTemp = new ReadOnlySpan<byte>(jsonBytesToRead);
            return JsonSerializer.Deserialize<AddressJson>(readOnlyTemp);
        }

        public bool CheckAddress(Address address)
        { 
            AddressJson addresses = GetAddresses();

            bool result;

            if (addresses != null && addresses.Streets.Any(p => p.id == address.StreetId
                                                && p.parentId == address.CityId
                                                && p.cityDistrictId == address.CityDistrictId))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void RemoveAddresses(List<AddressForSql> addressesToDelete)
        {
            byte[] jsonBytesToRead = File.ReadAllBytes("..\\Diia.Challenge.DAL\\Data\\addresses.json");
            var readOnlyTemp = new ReadOnlySpan<byte>(jsonBytesToRead);
            AddressJson addresses = JsonSerializer.Deserialize<AddressJson>(readOnlyTemp);

            foreach (AddressForSql address in addressesToDelete)
            {
                var StreetToDelete = addresses.Streets.Where(p => p.id == address.StreetId
                                                  && p.parentId == address.CityId
                                                  && p.cityDistrictId == address.CityDistrictId);
                foreach (StreetJson street in StreetToDelete)
                {
                    addresses.Streets.Remove(street);
                }

                var districtsToDelete = addresses.CityDistricts.Where(p => p.id == address.CityDistrictId
                                                && p.parentId == address.CityId);
                foreach (DistrictJson district in districtsToDelete)
                {
                    addresses.CityDistricts.Remove(district);
                }

                var citiesToDelete = addresses.Cities.Where(p => p.id == address.CityId).ToList();
                foreach (CityJson city in citiesToDelete)
                {
                    addresses.Cities.Remove(city);
                }

            }

            byte[] jsonBytesToWrite = JsonSerializer.SerializeToUtf8Bytes<AddressJson>(addresses);
            File.WriteAllBytes("..\\Diia.Challenge.DAL\\Data\\addresses.json", jsonBytesToWrite);
        }
    }
}
