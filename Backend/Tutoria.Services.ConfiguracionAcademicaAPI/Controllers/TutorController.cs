using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Tutoria.Services.ConfiguracionAcademicaAPI.Context;
using Tutoria.Services.ConfiguracionAcademicaAPI.Models;

namespace Tutoria.Services.ConfiguracionAcademicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly AppDbContext _configuracionAcademicaContext;

        public TutorController(AppDbContext appDbContext)
        {
            _configuracionAcademicaContext = appDbContext;
        }
        
        //===============================================================================
        //      Tutor
        //===============================================================================
        [HttpPost()]
        public async Task<IActionResult> AgregarTutor([FromBody] Tutor tutorObj)
        {
            if (tutorObj?.Equals(null) ?? true)
            {
                return BadRequest();
            }
            if (await CheckTutorExistAsync(tutorObj.Code!))
            {
                return BadRequest(new { Message = "El codigo ya existe!" });
            }


            //Agregar
            await _configuracionAcademicaContext.Tutores.AddAsync(tutorObj);
            //Guardar
            await _configuracionAcademicaContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Tutor registrado!"
            });

        }

        private Task<bool> CheckTutorExistAsync(string code)
            => _configuracionAcademicaContext.Tutores.AnyAsync(p => p.Code == code);

        //-------------------------------------------------------------------------------
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Tutor>>> GetTutores()
        {
            return await _configuracionAcademicaContext.Tutores.ToListAsync();
        }
        //-------------------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<Tutor>> GetTutorByCode(string id)
        {
            var tutor = await _configuracionAcademicaContext.Tutores.FirstAsync(x => x.Code == id);

            if (tutor == null)
                return NotFound();
            return tutor;
        }

        //-------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTutor(string id)
        {
            var tutor = await _configuracionAcademicaContext.Tutores.FirstAsync(x => x.Code == id);

            if (tutor == null)
                return NotFound();

            _configuracionAcademicaContext.Tutores.Remove(tutor);
            await _configuracionAcademicaContext.SaveChangesAsync();


            return Ok(new
            {
                Message = "Tutor fue eliminado!"
            });
        }
        //-------------------------------------------------------------------------------
        [HttpPut()]
        public async Task<IActionResult> PutTutor([FromBody] Tutor tutorActualizado)
        {
            if (tutorActualizado == null)
                return BadRequest();
            
            //Recuperar carga existente por su code
            var tutorExistente = await _configuracionAcademicaContext.Tutores.SingleOrDefaultAsync(x => x.Code == tutorActualizado.Code);
            if (tutorExistente == null)
            {
                return NotFound();//La carga no fue encontrado
            }

            //actualizar los campos especificos del tutor
            tutorExistente.NroCelular = tutorActualizado.NroCelular;
            tutorExistente.LugarReunion = tutorActualizado?.LugarReunion;
            tutorExistente.EnlaceReunion = tutorActualizado?.EnlaceReunion;

            //guardar los cambios en la base de datos
            try
            {
                await _configuracionAcademicaContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "El tutor fue actualizado con exito!"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar el tutor.");
            }
        }

        //-------------------------------------------------------------------------------
        // Enviar email de notificacion
        //-------------------------------------------------------------------------------
        //[HttpPost("enviarEmailNotificacionReserva/{email}")]
        //public async Task<IActionResult> SendEmail(string email)
        //{
        //    var tutor = await _configuracionAcademicaContext.Tutores.FirstOrDefaultAsync(a => a.Email == email);
        //    if (tutor is null)
        //    {
        //        return NotFound(new
        //        {
        //            StatusCode = 404,
        //            Message = "El email no existe."
        //        });
        //    }
        //    var tokenBytes = RandomNumberGenerator.GetBytes(64);
        //    var emailToken = Convert.ToBase64String(tokenBytes);

        //    user.ResetPasswordToken = emailToken;
        //    user.ResetPasswordExpiry = DateTime.UtcNow.AddMinutes(30);
        //    string from = _configuration["EmailSettings:From"];
        //    var emailModel = new EmailModel(email, "Reestablecer contraseña!", EmailBody.EmailStringBody(email, emailToken));
        //    _emailService.SendEmail(emailModel);
        //    _authContext.Entry(user).State = EntityState.Modified;
        //    await _authContext.SaveChangesAsync();
        //    return Ok(new
        //    {
        //        StatusCode = 200,
        //        Message = "El email fue enviado!"
        //    });
        //}
    }
}
