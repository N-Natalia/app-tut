namespace Tutoria.Services.ApiGateway.Dtos
{
    public class Person
    {
        public string? Code { get; set; }
        public string? Nombres { get; set; }
        public string? ApPaterno { get; set; }
        public string? ApMaterno { get; set; }
        public string? Email { get; set; }
        public User User { get; set; } = new User();
    }
}
