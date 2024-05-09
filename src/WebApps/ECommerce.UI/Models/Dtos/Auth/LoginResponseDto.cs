namespace ECommerce.UI.Models.Dtos.Auth
{
    public class LoginResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
