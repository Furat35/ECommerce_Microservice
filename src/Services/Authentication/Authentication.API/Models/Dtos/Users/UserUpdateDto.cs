namespace Authentication.API.Models.Dtos.Users
{
    public class UserUpdateDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string? Phone { get; set; }
    }
}
