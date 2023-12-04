namespace Tutoria.Services.SecurityAPI.Models.Dto
{
    public class RegistrationRequestDto
    {
        public string Code { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

    }
}
