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
        public int Id { get; set; }

        [Required]
        public DateTime creationDate { get; set; }

        [Required]
        [Precision(18,2)]
        public decimal money { get; set; }

        [Required]
        public bool isBlocked { get; set; }

        [Required]
        public int user_id { get; set; }

        [ForeignKey("user_id")]
        public virtual int User_id{ get; set; }
    }
}
