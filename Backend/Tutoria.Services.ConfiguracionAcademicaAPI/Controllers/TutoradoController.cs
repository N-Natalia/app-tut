using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutoria.Services.ConfiguracionAcademicaAPI.Context;
using Tutoria.Services.ConfiguracionAcademicaAPI.Models;

namespace Tutoria.Services.ConfiguracionAcademicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutoradoController : ControllerBase
    {
        private readonly AppDbContext _configuracionAcademicaContext;

        public TutoradoController(AppDbContext appDbContext)
        {
            _configuracionAcademicaContext = appDbContext;
        }
        
        //===============================================================================
        //      Tutorado
        //===============================================================================

        [HttpPost()]
        public async Task<IActionResult> AgregarTutorado([FromBody] Tutorado tutoradoObj)
        {
            if (tutoradoObj == null)
            {
                return BadRequest();
            }
            if (await CheckTutoradoExistAsync(tutoradoObj.Code!))
            {
                return BadRequest(new { Message = "El codigo ya existe!" });
            }

            //Agregar
            await _configuracionAcademicaContext.Tutorados.AddAsync(tutoradoObj);
            //Guardar
            await _configuracionAcademicaContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Tutorado registrado!"
            });

        }

        private Task<bool> CheckTutoradoExistAsync(string code)
            => _configuracionAcademicaContext.Tutorados.AnyAsync(p => p.Code == code);
        //-------------------------------------------------------------------------------
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Tutorado>>> GetTutorados()
        {
            return await _configuracionAcademicaContext.Tutorados.ToListAsync();
        }
        //-------------------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<Tutorado>> GetTutoradoByCode(string id)
        {
            var tutorado = await _configuracionAcademicaContext.Tutorados.FirstAsync(x => x.Code == id);

            if (tutorado == null)
                return NotFound();
            return tutorado;
        }
        //-------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTutorado(string id)
        {
            var tutorado = await _configuracionAcademicaContext.Tutorados.FirstAsync(x => x.Code == id);

            if (tutorado == null)
                return NotFound();

            _configuracionAcademicaContext.Tutorados.Remove(tutorado);
            await _configuracionAcademicaContext.SaveChangesAsync();


            return Ok(new
            {
                Message = "Tutorado fue eliminado!"
            });
        }
        //-------------------------------------------------------------------------------
        [HttpPut()]
        public async Task<IActionResult> UpdateTutorado([FromBody] Tutorado tutoradoActualizado)
        {
            if (tutoradoActualizado == null)
                return BadRequest();
            //Recuperar tutor existente por su code
            var tutorExistente = await _configuracionAcademicaContext.Tutorados.SingleOrDefaultAsync(x => x.Code == tutoradoActualizado.Code);
            if (tutorExistente == null)
            {
                return NotFound();//El tutorado no fue encontrado
            }

            //actualizar los campos especificos del tutorado
            tutorExistente.NroCelular = tutoradoActualizado.NroCelular;
            tutorExistente.Direccion = tutoradoActualizado.Direccion;
            tutorExistente.PersonReferencia = tutoradoActualizado.PersonReferencia;
            tutorExistente.NroCelularPersonaReferencia = tutoradoActualizado.NroCelularPersonaReferencia;

            //guardar los cambios en la base de datos
            try
            {
                await _configuracionAcademicaContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "El tutorado fue actualizado con exito!"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar el producto.");
            }
        }



        //-------------------------------------------------------------------------------
    }
}
