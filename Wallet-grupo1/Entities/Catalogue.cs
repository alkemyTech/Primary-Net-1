using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_grupo1.Entities
{
    public class Catalogue
    {
        [Column("catalogue_id")]
        public int Id { get; set; }
        
        [Column("catalogue_productDesc", TypeName = "VARCHAR(100)")]
        public string? Name { get; set; }   
        
        [Column("catalogue_image", TypeName = "VARCHAR(700)")]
        public string? Image { get; set; }   
        
        [Column("catalogue_points")]
        public int Points { get; set; }   
    }
}