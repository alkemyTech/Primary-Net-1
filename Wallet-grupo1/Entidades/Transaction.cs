using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_grupo1.Entidades;

public class Transaction
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public string Type { get; set; } = null!;

    public int Account_id { get; set; }

    public Account Account { get; set; } = null!;

    public int User_id;

    public User User { get; set; } = null!;

    public int To_account_id { get; set; }

    public Account To_account { get; set; } = null!;
}