using System.ComponentModel.DataAnnotations;

namespace Tutoria.Services.SecurityAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string  Code { get; set; }    
        public string Email { get; set; }
        public string Password { get; set; }    
        public string Token { get; set; }
        public string Role { get; set; }
        public string ResetPasswordToken { get; set; }  
        public DateTime ResetPasswordExpiry { get; set; }

    }
}
