
using Artillery.Data.Models.Enums;
using Artillery.DataProcessor.ExportDto;
using Artillery.Utilities;
using Newtonsoft.Json;

namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context.Shells
                .Where(s => s.ShellWeight > shellWeight)
                .Select(s => new
                {
                    ShellWeight = s.ShellWeight,
                    Caliber = s.Caliber,
                    Guns = s.Guns
                        .Where(s => ((int)s.GunType) == 3)
                        .Select(g => new
                        {
                            GunType = g.GunType.ToString(),
                            GunWeight = g.GunWeight,
                            BarrelLength = g.BarrelLength,
                            Range = g.Range > 3000 ? "Long-range" : "Regular range",
                        })
                        .OrderByDescending(g => g.GunWeight)
                        .ToArray()
                })
                .OrderBy(s => s.ShellWeight)
                .ToArray();

            return JsonConvert.SerializeObject(shells,Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var guns = context.Guns
                .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .Select(x => new ExportGunsDto()
                {
                    Manufacturer = x.Manufacturer.ManufacturerName,
                    GunType = x.GunType.ToString(),
                    BarrelLength = x.BarrelLength,
                    GunWeight = x.GunWeight,
                    Range = x.Range,
                    Countries = x.CountriesGuns
                        .Where(c => c.Country.ArmySize > 4500000)
                        .Select(c => new ExportGunCountries()
                        {
                            CountryName = c.Country.CountryName,
                            ArmySize = c.Country.ArmySize
                        })
                        .OrderBy(c => c.ArmySize)
                        .ToArray()
                })
                .OrderBy(g => g.BarrelLength)
                .ToArray();

            return xmlHelper.Serialize(guns, "Guns");
        }
    }
}
