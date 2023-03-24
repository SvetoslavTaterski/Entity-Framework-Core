using System.ComponentModel.DataAnnotations;
using System.Text;
using Artillery.Data.Models;
using Artillery.DataProcessor.ImportDto;
using Artillery.Utilities;
using System.Linq;
using Artillery.Data.Models.Enums;
using Newtonsoft.Json;

namespace Artillery.DataProcessor
{
    using Artillery.Data;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper xmlHelper = new XmlHelper();

            ImportCountriesDto[] countriesDtos = xmlHelper.Deserialize<ImportCountriesDto[]>(xmlString, "Countries");

            ICollection<Country> validCountries = new HashSet<Country>();

            foreach (var countryDto in countriesDtos)
            {
                if (!IsValid(countryDto))
                {
                    sb.AppendLine("Invalid data.");
                    continue;
                }

                Country validCountry = new Country()
                {
                    CountryName = countryDto.CountryName,
                    ArmySize = countryDto.ArmySize
                };

                validCountries.Add(validCountry);

                sb.AppendLine($"Successfully import {validCountry.CountryName} with {validCountry.ArmySize} army personnel.");
            }

            context.Countries.AddRange(validCountries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper xmlHelper = new XmlHelper();

            ImportManufacturersDto[] manufacturersDtos = xmlHelper.Deserialize<ImportManufacturersDto[]>(xmlString, "Manufacturers");

            ICollection<Manufacturer> validManufacturers = new HashSet<Manufacturer>();

            foreach (var manufacturerDto in manufacturersDtos)
            {
                var uniqueManufacturer = validManufacturers
                    .FirstOrDefault(m => m.ManufacturerName == manufacturerDto.ManufacturerName);

                if (!IsValid(manufacturerDto) || uniqueManufacturer != null)
                {
                    sb.AppendLine("Invalid data.");
                    continue;
                }

                Manufacturer validManufacturer = new Manufacturer()
                {
                    ManufacturerName = manufacturerDto.ManufacturerName,
                    Founded = manufacturerDto.Founded
                };

                validManufacturers.Add(validManufacturer);

                string[] data = validManufacturer.Founded.Split(", ", StringSplitOptions.RemoveEmptyEntries).ToArray();
                var last = data.Skip(Math.Max(0, data.Count() - 2)).ToArray();


                sb.AppendLine($"Successfully import manufacturer {validManufacturer.ManufacturerName} founded in {string.Join(", ", last)}.");
            }

            context.Manufacturers.AddRange(validManufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper xmlHelper = new XmlHelper();

            ImportShellsDto[] shellsDtos = xmlHelper.Deserialize<ImportShellsDto[]>(xmlString, "Shells");

            ICollection<Shell> validShells = new HashSet<Shell>();

            foreach (var shellDto in shellsDtos)
            {
                if (!IsValid(shellDto))
                {
                    sb.AppendLine("Invalid data.");
                    continue;
                }

                Shell shell = new Shell()
                {
                    ShellWeight = shellDto.ShellWeight,
                    Caliber = shellDto.Caliber
                };

                validShells.Add(shell);
                sb.AppendLine($"Successfully import shell caliber #{shell.Caliber} weight {shell.ShellWeight} kg.");
            }

            context.Shells.AddRange(validShells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            string[] validGunTypes = new string[]{"Howitzer","Mortar","FieldGun", "AntiAircraftGun", "MountainGun", "AntiTankGun"};

            var gunsDtos = JsonConvert.DeserializeObject<ImportGunsDto[]>(jsonString);

            ICollection<Gun> validGuns = new HashSet<Gun>();

            foreach (var gunDto in gunsDtos)
            {
                if (!IsValid(gunDto) || !validGunTypes.Contains(gunDto.GunType))
                {
                    sb.AppendLine("Invalid data.");
                    continue;
                }

                Gun gun = new Gun()
                {
                    ManufacturerId = gunDto.ManufacturerId,
                    GunWeight = gunDto.GunWeight,
                    BarrelLength = gunDto.BarrelLength,
                    NumberBuild = gunDto.NumberBuild,
                    Range = gunDto.Range,
                    ShellId = gunDto.ShellId,
                    GunType = (GunType)Enum.Parse(typeof(GunType),gunDto.GunType)
                };

                foreach (var countryDto in gunDto.Countries)
                {
                    if (!IsValid(countryDto))
                    {
                        sb.AppendLine("Invalid data.");
                        continue;
                    }

                    CountryGun country = new CountryGun()
                    {
                        CountryId = countryDto.Id,
                        Gun = gun
                    };

                    gun.CountriesGuns.Add(country);
                }

                validGuns.Add(gun);
                sb.AppendLine($"Successfully import gun {gun.GunType} with a total weight of {gun.GunWeight} kg. and barrel length of {gun.BarrelLength} m.");
            }

            context.Guns.AddRange(validGuns);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }

    }
}