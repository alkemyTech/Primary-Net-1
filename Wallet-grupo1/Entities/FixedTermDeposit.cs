using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wallet_grupo1.DTOs;

namespace Wallet_grupo1.Entities
{
    public class FixedTermDeposit
    {
        [Column("fixedterm_id")]
        public int Id { get; set; }

        [Required]
        [Column("fixedterm_amount", TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        
        [Required]
        [Column("fixedTerm_creationDate")]
        public DateTime CreationDate { get; set; }
        
        [Required]
        [Column("fixedTerm_closingDate")]
        public DateTime ClosingDate { get; set; }
        
        [Column("account_id")]
        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public FixedTermDeposit(int id, FixedTermDepositDto dto)
        {
            Id = id;
            ClosingDate = dto.ClosingDate;
            Amount = dto.Amount;
        }
        
        public FixedTermDeposit(){}
    }
}