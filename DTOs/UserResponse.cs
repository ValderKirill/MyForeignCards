namespace MyForeignCards.DTOs
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
