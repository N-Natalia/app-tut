using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutoria.Services.ConfiguracionAcademicaAPI.Context;
using Tutoria.Services.ConfiguracionAcademicaAPI.Models;

namespace Tutoria.Services.ConfiguracionAcademicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemestreController : ControllerBase
    {
        private readonly AppDbContext _configuracionAcademicaContext;

        public SemestreController(AppDbContext appDbContext)
        {
            _configuracionAcademicaContext = appDbContext;
        }
        
        
        //===============================================================================
        //      Semestre
        //===============================================================================
        [HttpPost()]
        public async Task<IActionResult> AgregarSemestre([FromBody] Semestre semestreObj)
        {
            if (semestreObj == null)
            {
                return BadRequest();
            }
            // Check Code
            if (await CheckIdSemestreExistAsync(semestreObj.IdSemestre))
                return BadRequest(new { Message = "El codigo ya existe!" });


            await _configuracionAcademicaContext.Semestres.AddAsync(semestreObj);
            await _configuracionAcademicaContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Semestre fue registrado!"
            });
        }
        private Task<bool> CheckIdSemestreExistAsync(int IdSemestre)
            => _configuracionAcademicaContext.Semestres.AnyAsync(x => x.IdSemestre == IdSemestre);
        //-------------------------------------------------------------------------------
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Semestre>>> GetSemestres()
        {
            return await _configuracionAcademicaContext.Semestres.ToListAsync();
        }

        //-------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSemestre(int id)
        {
            var semestre = await _configuracionAcademicaContext.Semestres.FirstAsync(x => x.IdSemestre == id);

            if (semestre == null)
                return NotFound();

            _configuracionAcademicaContext.Semestres.Remove(semestre);
            await _configuracionAcademicaContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Semestre fue eliminado!"
            });
        }
        //-------------------------------------------------------------------------------
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Semestre>> GetSemestre(int id)
        {
            var semestre = await _configuracionAcademicaContext.Semestres.FirstAsync(x => x.IdSemestre == id);

            if (semestre == null)
                return NotFound();
            return semestre;
        }
        //-------------------------------------------------------------------------------
        [HttpGet("Semestre-Activo")]
        public async Task<ActionResult<Semestre>> GetSemestreActivo()
        {
            var semestreActivo = await _configuracionAcademicaContext.Semestres.Where(x => x.Activo == true).FirstOrDefaultAsync();

            if (semestreActivo == null) return NotFound();

            return Ok(semestreActivo);

        }
        //-------------------------------------------------------------------------------
        [HttpPut("Desactivar/{id}")]
        public async Task<IActionResult> PutDesactivarSemestre(int id)
        {
            
            //Recuperar carga existente por su code
            var semestreExistente = await _configuracionAcademicaContext.Semestres.SingleOrDefaultAsync(x => x.IdSemestre == id);
            if (semestreExistente == null)
            {
                return NotFound();//El semestre no fue encontrado
            }

            //actualizar los campos especificos del administrador
            semestreExistente.Activo = false;

            //guardar los cambios en la base de datos
            try
            {
                await _configuracionAcademicaContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "El semestre fue desactivado con exito!"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al desactivar el semestre.");
            }
        }

    }
}
