using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wallet_grupo1.Entidades;

namespace Wallet_grupo1.Entidades;
public class Catalogue
{ 
    
    public int Id { get; set; }

    public string ProductDescription { get; set; } = null!;

    public string Image { get; set; } = null!;

    public double Points { get; set; }
}