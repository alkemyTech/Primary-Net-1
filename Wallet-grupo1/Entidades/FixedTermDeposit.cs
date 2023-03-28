using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Wallet_grupo1.Entidades
{
    public class FixedTermDeposit
    {
      
        public int Id { get; set; }

        public int User_id { get; set; }

        public User User { get; set; } = null!;

        public int To_account_id { get; set; }

        public Account To_account { get; set; } = null!;

        public decimal Amount { get; set; }

        public DateTime Creation_date { get; set; }

        public DateTime Closing_date { get; set; }
        
        
    }
}