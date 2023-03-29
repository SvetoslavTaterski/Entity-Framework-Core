using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VaporStore.Data.Models.Enums;

namespace VaporStore.Data.Models;

public class Card
{
    public Card()
    {
        Purchases = new HashSet<Purchase>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    public string Number { get; set; } = null!;

    [Required] 
    public string Cvc { get; set; } = null!;

    [Required]
    public CardType Type { get; set; }

    [ForeignKey(nameof(User))]
    [Required]
    public int UserId { get; set; }

    [Required]
    public User User { get; set; } = null!;

    public ICollection<Purchase> Purchases { get; set; }
}

