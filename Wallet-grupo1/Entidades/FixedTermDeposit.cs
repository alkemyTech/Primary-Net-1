using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Wallet_grupo1.Entidades
{
    [Table("FixedTermDeposits")]
    public class FixedTermDeposit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        
        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [NotMapped]
        public virtual User User { get; set; }

        
        [Column("to_account_id")]
        public int To_account_id { get; set; }

        [ForeignKey("To_account_id")]
        [NotMapped]
        public virtual Account To_account { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }
        
        [Column("creation_date")]
        public DateTime CreationDate { get; set; }

        [Column("closing_date")]
        public DateTime ClosingDate { get; set; }
        
        
    }
}