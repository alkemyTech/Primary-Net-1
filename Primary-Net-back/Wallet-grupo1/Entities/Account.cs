﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_grupo1.Entities
{
    public class Account
    {
        [Column("account_id")]
        public int Id { get; set; }

        [Required]
        [Column("account_creationDate")]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        [Required]
        [Column("account_money", TypeName = "decimal(18,2)")]
        public decimal Money { get; set; } = 0;

        [Required]
        [Column("account_isBlocked")]
        public bool IsBlocked { get; set; } = false;
        
        [Column("user_id")]
        public int UserId { get; set; }
        public User User { get; set; } 

        [NotMapped]
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        
        [NotMapped]
        public List<FixedTermDeposit> FixedTermDeposits { get; set; } = new List<FixedTermDeposit>();

        public Account() { }
    }
}
