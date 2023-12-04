using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutoria.Services.ConfiguracionAcademicaAPI.Context;
using Tutoria.Services.ConfiguracionAcademicaAPI.Models;

namespace Tutoria.Services.ConfiguracionAcademicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradorController : ControllerBase
    {
        private readonly AppDbContext _configuracionAcademicaContext;

        public AdministradorController(AppDbContext appDbContext)
        {
            _configuracionAcademicaContext = appDbContext;
        }
        
        //===============================================================================
        //      Administrador
        //===============================================================================
        [HttpPost()]
        public async Task<IActionResult> AgregarAdministrador([FromBody] Administrador administradorObj)
        {
            if (administradorObj == null)
            {
                return BadRequest();
            }
            if (await CheckAdministradorExistAsync(administradorObj.Code!))
            {
                return BadRequest(new { Message = "El codigo ya existe!" });
            }

            //Agregar
            await _configuracionAcademicaContext.Administradores.AddAsync(administradorObj);
            //Guardar
            await _configuracionAcademicaContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Administrador registrado!"
            });

        }

        private Task<bool> CheckAdministradorExistAsync(string code)
            => _configuracionAcademicaContext.Administradores.AnyAsync(p => p.Code == code);
        //-------------------------------------------------------------------------------
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Administrador>>> GetAdministradores()
        {
            return await _configuracionAcademicaContext.Administradores.ToListAsync();
        }
        //-------------------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<Administrador>> GetAdministradorByCode(string id)
        {
            var administrador = await _configuracionAcademicaContext.Administradores.FirstAsync(x => x.Code == id);

            if (administrador == null)
                return NotFound();
            return administrador;
        }
        //-------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdministrador(string id)
        {
            var administrador = await _configuracionAcademicaContext.Administradores.FirstAsync(x => x.Code == id);

            if (administrador == null)
                return NotFound();

            _configuracionAcademicaContext.Administradores.Remove(administrador);
            await _configuracionAcademicaContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Admnistrador fue eliminado!"
            });
        }
        //-------------------------------------------------------------------------------
        
        [HttpPut("Desactivar/{id}")]
        public async Task<IActionResult> PutDesactivarAdministrador(string id)
        {
            
            //Recuperar tutor existente por su code
            var adminstradorExistente = await _configuracionAcademicaContext.Administradores.SingleOrDefaultAsync(x => x.Code == id && x.Activo == true);
            if (adminstradorExistente == null)
            {
                return NotFound();//El tutorado no fue encontrado
            }

            //actualizar los campos especificos del administrador
            adminstradorExistente.Activo = false;

            //guardar los cambios en la base de datos
            try
            {
                await _configuracionAcademicaContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "El administrador fue desactivado con exito!"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar el administrador.");
            }
        }

        [HttpPut("Activar/{id}")]
        public async Task<IActionResult> PutActivarAdministrador(string id)
        {

            //Recuperar tutor existente por su code
            var adminstradorExistente = await _configuracionAcademicaContext.Administradores.SingleOrDefaultAsync(x => x.Code == id && x.Activo == false);
            if (adminstradorExistente == null)
            {
                return NotFound();//El tutorado no fue encontrado
            }

            //actualizar los campos especificos del administrador
            adminstradorExistente.Activo = true;

            //guardar los cambios en la base de datos
            try
            {
                await _configuracionAcademicaContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "El administrador fue activado con exito!"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar el administrador.");
            }
        }

    }
}
