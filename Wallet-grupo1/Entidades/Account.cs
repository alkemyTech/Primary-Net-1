﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_grupo1.Entidades
{
    public class Account
    {
       
        public int Id { get; set; }
       
        public DateTime Creation_date { get; set; }

        public decimal Money { get; set; }

        public bool Is_blocked { get; set; }

        public User User { get; set; } = null!;

        List<AccountTransaction> TransactionList { get; set; } = new List<AccountTransaction>();

        
    }
}
