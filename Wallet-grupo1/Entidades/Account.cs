using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_grupo1.Entidades
{
    public class Account
    {
        public int Id { get; set; }
        public DateOnly creationDate { get; set; }
        public double money { get; set; }
        public bool isBlocked { get; set; }
        public int user_id { get; set; }

        [ForeignKey("user_id")]
        public virtual int User_id{ get; set; }

    }
}
