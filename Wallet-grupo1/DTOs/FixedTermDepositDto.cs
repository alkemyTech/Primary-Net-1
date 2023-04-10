namespace Wallet_grupo1.DTOs;

public class FixedTermDepositDto
{
    public DateTime ClosingDate { get; set; }
    public decimal Amount { get; set; }
    public int AccountId { get; set; }
}