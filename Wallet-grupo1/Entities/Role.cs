using System.ComponentModel.DataAnnotations.Schema;
using Wallet_grupo1.DTOs;

namespace Wallet_grupo1.Entities;

public class Role
{
    [Column("role_id")]
    public int Id { get; set; }
        
    [Column("role_name", TypeName = "VARCHAR(100)")]
    public string Name { get; set; } 
        
    [Column("role_description", TypeName = "VARCHAR(100)")]
    public string? Description { get; set; }

    [Column("es_eliminado")]
    public bool EsEliminado { get; set; }

    public Role(RoleDto dto)
    {
        Name = dto.Name;
        Description = dto.Description;
    }
    
    public Role(){}
}

    
