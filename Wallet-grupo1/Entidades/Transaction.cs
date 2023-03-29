using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_grupo1.Entidades;

public class Transaction
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public TransactionType TransactionType { get; set; }

    public DateTime Date { get; set; }

    public Account Account { get; set; } = null!;
    
    public string Concept { get; set; }
}



public enum TransactionType
{
    Payment,
    Deposit
}