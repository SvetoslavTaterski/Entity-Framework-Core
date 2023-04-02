using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boardgames.Data.Models;

public class BoardgameSeller
{
    [ForeignKey(nameof(Boardgame))]
    [Required]
    public int BoardgameId { get; set; }

    public Boardgame Boardgame { get; set; }

    [ForeignKey(nameof(Seller))]
    [Required]
    public int SellerId { get; set; }

    public Seller Seller { get; set; }
}

