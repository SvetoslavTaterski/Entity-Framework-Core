using System.ComponentModel.DataAnnotations;
using Artillery.Common;

namespace Artillery.Data.Models;

public class Country
{
    public Country()
    {
        CountriesGuns = new HashSet<CountryGun>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.CountryNameMaxLength)]
    public string CountryName { get; set; } = null!;

    [Required] 
    public int ArmySize { get; set; }

    public ICollection<CountryGun> CountriesGuns { get; set; }
}

