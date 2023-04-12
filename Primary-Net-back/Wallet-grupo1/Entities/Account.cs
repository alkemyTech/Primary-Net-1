using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wallet_grupo1.DTOs;

namespace Wallet_grupo1.Entities
{
    public class Account
    {
        [Column("account_id")]
        public int Id { get; set; }
        
        [Required]
        [Column("account_creationDate")]
        public DateTime CreationDate { get; set; }

        [Required]
        [Column("account_money", TypeName = "decimal(18,2)")]
        public decimal Money { get; set; }

        [Required]
        [Column("account_isBlocked")]
        public bool IsBlocked { get; set; }
        
        [Column("user_id")]
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        [NotMapped] 
        public List<Transaction> Transactions { get; set; } = null!;
        
        [NotMapped] 
        public List<FixedTermDeposit> FixedTermDeposits { get; set; } = null!;
        
        /// <summary>
        /// Data transfer object asociados a una cuenta.
        /// </summary>
        public Account(int id, AccountDto dto)
        {
            Id = id;
            Money = dto.Money;
            IsBlocked = dto.IsBlocked;
        }
        
        /// <summary>
        /// Data transfer object asociados a una cuenta.
        /// </summary>
        public Account()
        {
        }
    }
}
