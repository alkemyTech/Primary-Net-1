using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_grupo1.Entidades;

[Table("Transactions")]
public class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("transaction_id")]
    public int Id { get; set; }
    
    [Required]
    [Column("amount")]
    public decimal Amount { get; set; }
    
    [Required]
    [Column("date")]
    public DateTime Date { get; set; }
    
    [Required]
    [Column("type")]
    public string Type { get; set; }
    
    [Required]
    [Column("account_id")]
    public int Account_id { get; set; }
    
    [ForeignKey("Account_id")]
    public virtual Account Account { get; set; }
    
    [Required]
    [Column("user_id")]
    public int User_id;
    
    [ForeignKey("User_id")]
    public virtual User User { get; set; }
    
    [Required]
    [Column("to_account_id")]
    public int To_account_id { get; set; }
    
    [ForeignKey("To_account_id")]
    public virtual Account To_account { get; set; }
}