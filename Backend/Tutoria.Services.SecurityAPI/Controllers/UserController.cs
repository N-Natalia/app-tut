using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Tutoria.Services.SecurityAPI.Context;
using Tutoria.Services.SecurityAPI.Helpers;
using Tutoria.Services.SecurityAPI.Models;
using Tutoria.Services.SecurityAPI.Models.Dto;
using Tutoria.Services.SecurityAPI.UtilityService;

namespace Tutoria.Services.SecurityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public UserController( AppDbContext appDbContext, 
                                IConfiguration configuration,
                                IEmailService emailService) 
        {
            _authContext = appDbContext;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto userObj)
        {
            if (userObj == null)
            {
                return BadRequest();
            }
            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.Code == userObj.Code);
            if (user == null)
            {
                return NotFound( new {Message= "Usuario no fue encontrado!"} );
            }

            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                return BadRequest(new { Message = "La contraseña es incorrecta." });
            }
            // Create jwtToken
            user.Token = CreateJwt(user);

            return Ok( new {

                User = userObj,
                ok = true,
                Token= user.Token,
                Message = "Login exitoso!"
            });
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User userObj)
        {
            if( userObj == null)
            {
                return BadRequest();
            }
            // Check Code
            if (await CheckCodeExistAsync(userObj.Code))
                return BadRequest(new { Message = "El codigo ya existe!" });


            // Check Email
            //if (await CheckEmailExistAsync(userObj.Email))
            //    return BadRequest(new { Message = "El email ya existe!" });


            // Check password Strength
            var pass = CheckPasswordStrength(userObj.Password);
            if(!string.IsNullOrEmpty(pass))
                return BadRequest(new { Message = pass.ToString() });


            // hash password
            userObj.Password = PasswordHasher.HashPassword(userObj.Password);
            //userObj.Role = "User";
            userObj.Token = userObj.Token;

            await _authContext.Users.AddAsync(userObj);
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Usuario registrado!"
            });
        }
 
        private Task<bool> CheckCodeExistAsync(string code)
            => _authContext.Users.AnyAsync(x => x.Code == code);

        private Task<bool> CheckEmailExistAsync(string email)
            => _authContext.Users.AnyAsync(x => x.Email == email);


        private string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder(); 
            if(password.Length < 8)
                sb.Append("Longitud minima de la contraseña son 8 caracteres." + Environment.NewLine);
            if (!(Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[A-Z]")
                && Regex.IsMatch(password, "[0-9]")))
                sb.Append("La contraseña debe ser alphanumerica." + Environment.NewLine);
            if(!Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,-,=,´]"))
                sb.Append("La contraseña debe contener caracteres especiales." + Environment.NewLine);
            return sb.ToString();


        }

        private string CreateJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("secretKey=stringRemenber");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier,user.Code)
                
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddDays(1),//Tiempo de vencimiento del token
                SigningCredentials = credentials,
                
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);   
            return jwtTokenHandler.WriteToken(token);
        }

        [Authorize]//proteger api
        [HttpGet]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            return Ok(await _authContext.Users.ToListAsync());
        }

        //reset password
        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> SendEmail(string email)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(a => a.Email == email);
            if( user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "El email no existe."
                });
            }
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);

            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.UtcNow.AddMinutes(30);
            string from = _configuration["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Reestablecer contraseña!", EmailBody.EmailStringBody(email, emailToken));
            _emailService.SendEmail(emailModel);
            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "El email fue enviado!"
            });
        }


        //Email Notificacion reserva
        [HttpPost("send-reserva-notificacion-email/{email}")]
        public async Task<IActionResult> SendNotificacionEmail(string email)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(a => a.Email == email);
            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "El email no existe."
                });
            }
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);

            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.UtcNow.AddMinutes(30);
            string from = _configuration["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Tienes una nueva reserva!", EmailNotificationReservaBody.EmailStringBody(email, emailToken));
            _emailService.SendEmail(emailModel);
            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "El email fue enviado!"
            });
        }

        //Email Notificacion programación
        [HttpPost("send-programacion-obligatoria-notificacion-email/{email}")]
        public async Task<IActionResult> SendProgramaciónObligatoriaNotificacionEmail(string email)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(a => a.Email == email);
            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "El email no existe."
                });
            }
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);

            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.UtcNow.AddMinutes(30);
            string from = _configuration["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Tienes una nueva programación de tutoría obligatoria!", EmailNotificationProgramacionROBody.EmailStringBody(email, emailToken));
            _emailService.SendEmail(emailModel);
            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "El email fue enviado!"
            });
        }

        //Email Notificacion programación
        [HttpPost("send-confirmation-reserva-notificacion-email/{email}")]
        public async Task<IActionResult> SendConfirmationReservaNotificacionEmail(string email)
        {
            var user = await _authContext.Users.FirstOrDefaultAsync(a => a.Email == email);
            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "El email no existe."
                });
            }
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);

            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.UtcNow.AddMinutes(30);
            string from = _configuration["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Reserva confirmado!", EmalNotificationConfirmacionReservaBody.EmailStringBody(email, emailToken));
            _emailService.SendEmail(emailModel);
            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "El email fue enviado!"
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassowrd(ResetPasswordDto resetPasswordDto)
        {
            
            var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");
            var user = await _authContext.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Email == resetPasswordDto.Email);

            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "El usuario no existe."
                });
            }

            var tokenCode = user.ResetPasswordToken;
            DateTime emailTokenExpiry = user.ResetPasswordExpiry;

            if  (!(tokenCode.ToString().Equals(newToken.ToString())) || (emailTokenExpiry < DateTime.UtcNow))
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Enlace de reestablecimiento invalido."
                });
            }

            user.Password = PasswordHasher.HashPassword(resetPasswordDto.NewPassword);
            _authContext.Entry(user).State = EntityState.Modified;
            await _authContext.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Message = "La contraseña fue reestablecida exitosamente."
            });


        }

    }
}
