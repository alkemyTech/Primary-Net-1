using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wallet_grupo1.DTOs;

namespace Wallet_grupo1.Entities
{
    public class Transaction
    {
        [Column("transaction_id")]
        public int Id { get; set; }
        
        [Required]
        [Column("transaction_type", TypeName = "VARCHAR(15)")]
        public TransactionType Type { get; set; }

        [Required]
        [Column("transaction_amount", TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [Column("transaction_concept", TypeName = "VARCHAR(100)")]
        public string? Concept { get; set; }
        
        [Required]
        [Column("transaction_date")]
        public DateTime Date { get; set; }
        
        [Column("account_id")]
        public int? AccountId { get; set; }
        public Account? Account { get; set; }
        
        [Column("to_account_id")]
        public int? ToAccountId { get; set; }
        public Account? ToAccount { get; set; }
     
        /// <summary>
        /// Data transfer objects asociados a una transaccion.
        /// </summary>
        public Transaction(int id, TransactionDto dto)
        {
            Id = id;
            Concept = dto.Concept;
        }
        
        /// <summary>
        /// Data transfer objects asociados a una transaccion.
        /// </summary>
        public Transaction()
        {
        }
    }
 
    public enum TransactionType
    {
        Deposit,
        Payment
    }
}