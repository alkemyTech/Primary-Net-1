﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_grupo1.Entidades
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public string first_name { get; set; } = null!;

        
        public string last_name { get; set;} = null!;

        public string email { get; set; } = null!;

        public string password { get; set; } = null!;

        public double points { get; set; }

        public int rol_Id { get; set; }

    }
}
