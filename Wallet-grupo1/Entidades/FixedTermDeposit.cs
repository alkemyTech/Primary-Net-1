using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Wallet_grupo1.Entidades
{
    public class FixedTermDeposit
    {
      
        public int Id { get; set; }

        public Account Account { get; set; } = null!;

        public decimal Amount { get; set; }

        public DateTime Creation_date { get; set; }

        public DateTime Closing_date { get; set; }
        
        
    }
}