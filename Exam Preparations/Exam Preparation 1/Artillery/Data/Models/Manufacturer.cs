using System.ComponentModel.DataAnnotations;
using Artillery.Common;

namespace Artillery.Data.Models;

public class Manufacturer
{
    public Manufacturer()
    {
        Guns = new HashSet<Gun>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.ManufacturerNameMaxLength)]
    public string ManufacturerName { get; set; } = null!;

    [Required]
    [MaxLength(ValidationConstants.FoundedMaxLength)]
    public string Founded { get; set; } = null!;

    public ICollection<Gun> Guns { get; set; }
}

