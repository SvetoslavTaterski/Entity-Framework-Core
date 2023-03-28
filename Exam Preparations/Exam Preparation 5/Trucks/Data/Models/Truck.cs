using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Trucks.Data.Models.Enums;

namespace Trucks.Data.Models;

public class Truck
{
    public Truck()
    {
        ClientsTrucks = new HashSet<ClientTruck>();
    }

    [Key]
    public int Id { get; set; }

    public string? RegistrationNumber { get; set; }

    [Required] 
    public string VinNumber { get; set; } = null!;

    [Required]
    public int TankCapacity { get; set; }

    [Required]
    public int CargoCapacity  { get; set; }

    [Required]
    public CategoryType CategoryType { get; set; }

    [Required]
    public MakeType MakeType { get; set; }

    [ForeignKey(nameof(Despatcher))]
    public int DespatcherId { get; set; }

    public Despatcher Despatcher { get; set; }

    public ICollection<ClientTruck> ClientsTrucks { get; set; }
}

