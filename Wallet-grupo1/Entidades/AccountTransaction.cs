namespace Wallet_grupo1.Entidades
{
    public class AccountTransaction
    {
        public int Id { get; set; }
        public Account Account { get; set; } = null!;

        public Account To_account { get; set; } = null!;

        public Transaction Transaction { get; set; } = null!;

        public double Amount { get; set; }

        public DateTime? CreatedAt { get; set; }    
    }
}
