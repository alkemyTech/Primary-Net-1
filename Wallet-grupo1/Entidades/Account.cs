using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_grupo1.Entidades
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("creationDate")]
        public DateTime CreationDate { get; set; }

        [Required]
        [Precision(18,2)]
        [Column("money")]
        public decimal Money { get; set; }

        [Required]
        [Column("isBlocked")]
        public bool IsBlocked { get; set; }


        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey("user_id")]
        [NotMapped]
        public virtual User User{ get; set; }
    }
}
