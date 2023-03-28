﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_grupo1.Entidades;

[Table("Roles")]
public class Role
{ 
    public int Id { get; set; }

    public RoleType Name { get; set; }

    public string Description { get; set; } = null!;

}

public enum RoleType
{
    Admin,
    Regular
}