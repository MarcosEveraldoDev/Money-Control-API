namespace WebApplication2.Models.DTOs
{
    public class RegisterUserDto
    {

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string CPF { get; set; }
        public DateOnly DateBirth { get; set; }

    }
}
