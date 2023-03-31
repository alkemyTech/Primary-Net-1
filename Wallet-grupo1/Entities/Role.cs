using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_grupo1.Entities
{
    public class Role
    {
        [Column("role_id")]
        public int Id { get; set; }
        
        [Column("role_name", TypeName = "VARCHAR(100)")]
        public string Name { get; set; } 
        
        [Column("role_description", TypeName = "VARCHAR(100)")]
        public string? Description { get; set; }
    }
    
}