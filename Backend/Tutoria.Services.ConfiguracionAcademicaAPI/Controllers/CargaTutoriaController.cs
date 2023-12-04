using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutoria.Services.ConfiguracionAcademicaAPI.Context;
using Tutoria.Services.ConfiguracionAcademicaAPI.Models;
using Tutoria.Services.ConfiguracionAcademicaAPI.Models.Dto;

namespace Tutoria.Services.ConfiguracionAcademicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargaTutoriaController : ControllerBase
    {
        private readonly AppDbContext _configuracionAcademicaContext;

        public CargaTutoriaController(AppDbContext appDbContext)
        {
            _configuracionAcademicaContext = appDbContext;
        }
        
        //===============================================================================
        //      Carga Tutoria
        //===============================================================================
        [HttpPost()]
        public async Task<ActionResult<Semestre>> AgregarCargaTutoria([FromBody] RequestCargaDto cargaObj)
        {
            if (cargaObj == null)
            {
                return BadRequest();
            }

            // Check IdTutor
            if (!await CheckIdTutorExistAsync(cargaObj.IdTutor!))
                return BadRequest(new { Message = "El codigo de tutor no fue encontrado!" });


            // Check IdTutorado
            if (!await CheckIdTutoradoExistAsync(cargaObj.IdTutorado!))
                return BadRequest(new { Message = "El codigo de tutorado no fue encontrado!" });

            // Check Code
            if (!await CheckIdSemestExistAsync(cargaObj.IdSemestre))
                return BadRequest(new { Message = "El codigo de semestre no fue encontrado!" });

            // Crear una instancia de CargaTutoria
            var cargaTutoria = new CargaTutoria
            {
                IdTutor = cargaObj.IdTutor,
                IdTutorado = cargaObj.IdTutorado,
                IdSemestre = cargaObj.IdSemestre,
                Estado = cargaObj.Estado
            };

            await _configuracionAcademicaContext.CargasTutoria.AddAsync(cargaTutoria);
            await _configuracionAcademicaContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "La carga de tutoria fue registrado!"
            });
        }
        private Task<bool> CheckIdCargaExistAsync(int IdCargaTutoria)
            => _configuracionAcademicaContext.CargasTutoria.AnyAsync(x => x.IdCargaTutoria == IdCargaTutoria);
        private Task<bool> CheckIdTutorExistAsync(string IdTutor)
            => _configuracionAcademicaContext.Tutores.AnyAsync(x => x.Code == IdTutor);
        private Task<bool> CheckIdTutoradoExistAsync(string IdTutorado)
            => _configuracionAcademicaContext.Tutorados.AnyAsync(x => x.Code == IdTutorado);
        private Task<bool> CheckIdSemestExistAsync(int IdSemestre)
            => _configuracionAcademicaContext.Semestres.AnyAsync(x => x.IdSemestre == IdSemestre);
        //-------------------------------------------------------------------------------
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<RequestCargaDto>>> GetCargas()
        {
            //lista de cargas
            var cargasDto = await _configuracionAcademicaContext.CargasTutoria
                .Select(x => new RequestCargaDto
                {
                    IdTutor = x.IdTutor!,
                    IdTutorado = x.IdTutorado!,
                    IdSemestre = x.IdSemestre,
                    Estado = x.Estado
                }).ToListAsync();
            if (cargasDto.Count == 0)
            {
                return NotFound();//No se encontraron registros
            }
            return cargasDto;

        }
        //-------------------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<CargaTutoria>> GetCargaByCode(int id)
        {
            var carga = await _configuracionAcademicaContext.CargasTutoria.FirstAsync(x => x.IdCargaTutoria == id);

            if (carga == null)
                return NotFound();
            return carga;
        }
        //-------------------------------------------------------------------------------
        [HttpGet("Lista-Activos")]
        public async Task<ActionResult<IEnumerable<RequestCargaDto>>> GetCargasActivos()
        {
            //lista de cargas
            var cargasDto = await _configuracionAcademicaContext.CargasTutoria
                .Where(x => x.Estado == true)
                .Select(x => new RequestCargaDto
                {
                    IdTutor = x.IdTutor!,
                    IdTutorado = x.IdTutorado!,
                    IdSemestre = x.IdSemestre,
                    Estado = x.Estado
                }).ToListAsync();
            if (cargasDto.Count == 0)
            {
                return NotFound();//No se encontraron registros
            }
            return cargasDto;
        }


        //-------------------------------------------------------------------------------
        [HttpGet("CodeTutoradosByCodeTutor/{id}")]//carga activa
        public async Task<ActionResult<IEnumerable<ResponseListCodeTutoradosByCodeTutorDto>>> GetListTutoradosByCodeTutor(string id)
        {
            //lista de cargas
            var listCodeTutorados = await _configuracionAcademicaContext.CargasTutoria
                .Where(x => x.Estado == true && x.IdTutor == id)
                .Select(x => new ResponseListCodeTutoradosByCodeTutorDto
                {
                    IdTutorado = x.IdTutorado!
                })
                .ToListAsync();
            if (listCodeTutorados.Count == 0)
            {
                return NotFound();//No se encontraron registros
            }
            return listCodeTutorados;
        }
        //-------------------------------------------------------------------------------
        [HttpGet("CodeTutorByCodeTutorado/{id}")]//carga activa
        public async Task<ActionResult<ResponseCodeTutorDto>> GetCodeTutorByCodeTutorado(string id)
        {

            var codeTutor = await _configuracionAcademicaContext.CargasTutoria
                .Where(x => x.Estado == true && x.IdTutorado == id)
                .Select(x => x.IdTutor)
                .FirstOrDefaultAsync();
            if (codeTutor == null)
            {
                return NotFound();//No se encontraron registros
            }
            return (
                new ResponseCodeTutorDto
                {
                    CodeTutor = codeTutor
                }
            );
        }
        //-------------------------------------------------------------------------------
        [HttpGet("IdCargaByCodeTutorado/{id}")]//carga activa
        public async Task<ActionResult<ResponseIdCargaDto>> GetIdCargaByCodeTutorado(string id)
        {

            var codeCarga = await _configuracionAcademicaContext.CargasTutoria
                .Where(x => x.Estado == true && x.IdTutorado == id)
                .Select(x => x.IdCargaTutoria)
                .FirstOrDefaultAsync();
            if (codeCarga == 0)
            {
                return NotFound();//No se encontraron registros
            }
            return (new ResponseIdCargaDto
            { 
                IdCarga = codeCarga
            });
        }

        //-------------------------------------------------------------------------------
        [HttpGet("CodeTutoradoByIdCarga/{id}")]//carga activa
        public async Task<ActionResult<ResponseCodeTutoradoDto>> GetCodeTutoradoByIdCarga(int id)
        {

            var codeTutorado = await _configuracionAcademicaContext.CargasTutoria
                .Where(x => x.Estado == true && x.IdCargaTutoria == id)
                .Select(x => x.IdTutorado)
                .FirstOrDefaultAsync();
            if (codeTutorado == null)
            {
                return NotFound();//No se encontraron registros
            }
            return (new ResponseCodeTutoradoDto { CodeTutorado = codeTutorado });
        }

        //-------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarga(int id)
        {
            var carga = await _configuracionAcademicaContext.CargasTutoria.FirstAsync(x => x.IdCargaTutoria == id);

            if (carga == null)
                return NotFound();

            _configuracionAcademicaContext.CargasTutoria.Remove(carga);
            await _configuracionAcademicaContext.SaveChangesAsync();


            return Ok(new
            {
                Message = "Carga fue eliminado!"
            });
        }
        
        //-------------------------------------------------------------------------------
        
        [HttpPut("Desactivar/{id}")]
        public async Task<IActionResult> PutDesactivarCargaTutoria(int id)
        {
           
            
            //Recuperar carga existente por su code
            var cargaExistente = await _configuracionAcademicaContext.CargasTutoria.SingleOrDefaultAsync(x => x.IdCargaTutoria == id);
            if (cargaExistente == null)
            {
                return NotFound();//La carga no fue encontrado
            }

            //actualizar los campos especificos del administrador
            cargaExistente.Estado = false;

            //guardar los cambios en la base de datos
            try
            {
                await _configuracionAcademicaContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "La carga fue desactivado con exito!"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al desactivar la carga.");
            }
        }
        
        [HttpPut("DesactivarCargas")]
        public async Task<IActionResult> DesactivarCargas()
        {
            var cargasActivos = await _configuracionAcademicaContext.CargasTutoria
                .Where(x => x.Estado == true)
                .ToListAsync();
            if (cargasActivos.Count == 0)
                return NotFound();

            //Actualizar activo por inactivo
            cargasActivos.ForEach(x => x.Estado = false);

            //Guardar cambios
            await _configuracionAcademicaContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Las cargas activas fueron desactivados correctamente."
            });
        }
    }
}
