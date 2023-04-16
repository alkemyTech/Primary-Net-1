namespace Wallet_grupo1.DTOs
{
    public class UserLoginDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Password { get; set; } = null!;   

        public string Email { get; set; } = null!;

        public string Token { get; set; }

        public bool isAdmin { get; set; }
    }
}
