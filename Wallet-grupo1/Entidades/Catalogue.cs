using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("Catalogue")]
public class Catalogue
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    [Required]
    [Column("to_account_id")]
    public int to_account_id { get; set; }

    [ForeignKey("To_account_id")]
    public virtual Account To_account { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ClosingDate { get; set; }


}