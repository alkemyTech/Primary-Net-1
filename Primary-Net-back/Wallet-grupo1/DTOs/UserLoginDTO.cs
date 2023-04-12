namespace Wallet_grupo1.DTOs
{
    public class UserLoginDTO
    {
        public string Name { get; set; } = null!;

        public string Password { get; set; } = null!;   

        public string Email { get; set; } = null!;

        public string Token { get; set; }

        public bool isAdmin { get; set; }
    }
}
