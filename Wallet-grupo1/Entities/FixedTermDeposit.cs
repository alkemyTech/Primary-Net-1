using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wallet_grupo1.DTOs;

namespace Wallet_grupo1.Entities
{
    /// <summary>
    /// Entidad Plazo fijo, compuesta por un campo identificador, una cantidad de dinero invertida, una fecha
    /// de inicio y otra de finalización para cobrar los intereses y el id de la cuenta asociada al mismo.
    /// </summary>
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

        /// <summary>
        /// Data transfer objects asociados al plazo fijo.
        /// </summary>
        public FixedTermDeposit(int id, FixedTermDepositDto dto)
        {
            Id = id;
            ClosingDate = dto.ClosingDate;
            Amount = dto.Amount;
        }
        
        /// <summary>
        /// Data transfer objects asociados al plazo fijo.
        /// </summary>
        public FixedTermDeposit(FixedTermDepositDto dto)
        {
            ClosingDate = dto.ClosingDate;
            Amount = dto.Amount;
            AccountId = dto.OwnerAccountId;
        }
        
        /// <summary>
        /// Data transfer objects asociados al plazo fijo.
        /// </summary>
        public FixedTermDeposit(){}
    }
}