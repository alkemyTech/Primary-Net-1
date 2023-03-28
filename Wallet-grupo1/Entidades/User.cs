using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_grupo1.Entidades
{
    public class User
    {
        public int Id { get; set; }

        public string First_name { get; set; } = null!;
        
        public string Last_name { get; set;} = null!;
        
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
        
        public double Points { get; set; }
        
        public Role Rol { get; set; } = null!;

        public int Rol_Id { get; set; }

    }
}
