using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Context;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Dto;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Enums;

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorHorarioDisponibleController : ControllerBase
    {
        private readonly AppDbContext _sesionTutoriaAcademicaContext;

        public TutorHorarioDisponibleController(AppDbContext context)
        {
            _sesionTutoriaAcademicaContext = context;
        }

        //===============================================================================
        //          HORARIO TUTOR DISPONIBLE
        //===============================================================================
        // Agregar Horario
        //-------------------------------------------------------------------------------
        [HttpPost()]
        public async Task<IActionResult> AgregarTutorHorarioDisponible([FromBody] TutorHorarioDisponible tutorHorarioDispObj)
        {
            if (tutorHorarioDispObj == null)
            {
                return BadRequest();
            }
            if (await CheckHorarioExistAsync(tutorHorarioDispObj.IdTutor!, tutorHorarioDispObj.Dia!, tutorHorarioDispObj.Hora))
            {
                return BadRequest(new { message = "Este horario ya fue registrado por usted!" });
            }
            if (await CheckHorarioExistDesactivadoAsync(tutorHorarioDispObj.IdTutor!, tutorHorarioDispObj.Dia!, tutorHorarioDispObj.Hora))
            {
                //Recuperar tutorHorarioDisponibleActualizado existente por su code
                var tutorHorarioDisponibleDesactivado = await _sesionTutoriaAcademicaContext.TutorHorariosDisponible.SingleOrDefaultAsync(x => x.Dia == tutorHorarioDispObj.Dia && x.Hora == tutorHorarioDispObj.Hora);
                if (tutorHorarioDisponibleDesactivado == null)
                {
                    return NotFound();//El tutor horario disponible no fue encontrado
                }

                //actualizar los campos especificos del tutorado
                tutorHorarioDisponibleDesactivado.Activo = true;
                //guardar los cambios en la base de datos
                try
                {
                    await _sesionTutoriaAcademicaContext.SaveChangesAsync();
                    return Ok(new
                    {
                        Message = "Tutor horario disponible fue actualizado con exito!"
                    });
                }
                catch (DbUpdateConcurrencyException)
                {
                    return StatusCode(500, "Error al actualizar el horario disponible del tutor.");
                }
                
            }


            //Agregar
            await _sesionTutoriaAcademicaContext.TutorHorariosDisponible.AddAsync(tutorHorarioDispObj);
            //Guardar
            await _sesionTutoriaAcademicaContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Tutor horario disponible registrado!"
            });

        }

        private Task<bool> CheckHorarioExistAsync(string codeTutor, string dia, TimeOnly Hora)
            => _sesionTutoriaAcademicaContext.TutorHorariosDisponible.AnyAsync(x => x.IdTutor == codeTutor && x.Dia == dia && x.Hora == Hora && x.Activo== true);
        private Task<bool> CheckHorarioExistDesactivadoAsync(string codeTutor, string dia, TimeOnly Hora)
            => _sesionTutoriaAcademicaContext.TutorHorariosDisponible.AnyAsync(x => x.IdTutor == codeTutor && x.Dia == dia && x.Hora == Hora && x.Activo== false);

        


        //-------------------------------------------------------------------------------
        // Update Horario
        //-------------------------------------------------------------------------------
        [HttpPut()]
        public async Task<IActionResult> UpdateTutorHoraioDisponible([FromBody] TutorHorarioDisponible tutorHorarioDisponibleActualizado)
        {
            if (tutorHorarioDisponibleActualizado == null)
                return BadRequest();
            //Recuperar tutorHorarioDisponibleActualizado existente por su code
            var tutorHorarioDisponibleExistente = await _sesionTutoriaAcademicaContext.TutorHorariosDisponible.SingleOrDefaultAsync(x => x.IdHorario == tutorHorarioDisponibleActualizado.IdHorario);
            if (tutorHorarioDisponibleExistente == null)
            {
                return NotFound();//El tutor horario disponible no fue encontrado
            }

            //actualizar los campos especificos del tutorado
            tutorHorarioDisponibleExistente.Hora = tutorHorarioDisponibleActualizado.Hora;
            tutorHorarioDisponibleExistente.Duracion = tutorHorarioDisponibleActualizado.Duracion;
            tutorHorarioDisponibleExistente.Dia = tutorHorarioDisponibleActualizado.Dia;
            tutorHorarioDisponibleExistente.Tipo = tutorHorarioDisponibleActualizado.Tipo;
            tutorHorarioDisponibleExistente.Activo = tutorHorarioDisponibleActualizado.Activo;

            //guardar los cambios en la base de datos
            try
            {
                await _sesionTutoriaAcademicaContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "Tutor horario disponible fue actualizado con exito!"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar el horario disponible del tutor.");
            }
        }

        //-------------------------------------------------------------------------------
        //  TutorHorarioDisponible-Delete
        //-------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTutorHorarioDisponible(int id)
        {
            var TutorHorarioDisponible = await _sesionTutoriaAcademicaContext.TutorHorariosDisponible.FirstAsync(x => x.IdHorario == id);

            if (TutorHorarioDisponible == null)
                return NotFound();

            //Eliminar objeto
            _sesionTutoriaAcademicaContext.TutorHorariosDisponible.Remove(TutorHorarioDisponible);
            //Guardar cambios
            await _sesionTutoriaAcademicaContext.SaveChangesAsync();


            return Ok(new
            {
                Message = "Tutor horario disponible fue eliminado!"
            });
        }

        //-------------------------------------------------------------------------------
        //  Get Tutor Horario Disponible lista
        //-------------------------------------------------------------------------------
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ResponseTutorHorarioDisponibleDto>>> GetTutoresHorariosDiponibles()
        {
            var tipoReunionType = typeof(TipoReunion);
            return await _sesionTutoriaAcademicaContext.TutorHorariosDisponible
                            .Select(tutorHorarioDisponible => new ResponseTutorHorarioDisponibleDto
                            {

                                IdHorario = tutorHorarioDisponible.IdHorario,
                                IdTutor = tutorHorarioDisponible.IdTutor,
                                Hora = tutorHorarioDisponible.Hora,
                                Duracion = tutorHorarioDisponible.Duracion,
                                Dia = tutorHorarioDisponible.Dia,
                                Tipo = Enum.GetName(tipoReunionType, tutorHorarioDisponible.Tipo),
                                Activo = tutorHorarioDisponible.Activo
                            }).ToListAsync();
        }


        //-------------------------------------------------------------------------------
        //  Get Tutor Horario Disponible by Code Horario
        //-------------------------------------------------------------------------------

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseTutorHorarioDisponibleDto>> GetTutorHorarioDisponibleById(int id)
        {
            var tutorHorarioDisponible = await _sesionTutoriaAcademicaContext.TutorHorariosDisponible.FirstOrDefaultAsync(x => x.IdHorario == id);

            if (tutorHorarioDisponible == null)
                return NotFound();

            // Obtener el valor real del enum tipo
            string tipoReal = Enum.GetName(typeof(TipoReunion), tutorHorarioDisponible.Tipo)!;

            return (new ResponseTutorHorarioDisponibleDto
            {
                IdHorario = tutorHorarioDisponible.IdHorario,
                IdTutor = tutorHorarioDisponible.IdTutor,
                Hora = tutorHorarioDisponible.Hora,
                Duracion = tutorHorarioDisponible.Duracion,
                Dia = tutorHorarioDisponible.Dia,
                Tipo = tipoReal,
                Activo = tutorHorarioDisponible.Activo
            });

        }
        //-------------------------------------------------------------------------------
        //  Get Tutor Horario Disponible by Code Tutor
        //-------------------------------------------------------------------------------

        [HttpGet("ListaHorariosByCodeTutor/{id}")]
        public async Task<ActionResult<IEnumerable<ResponseTutorHorarioDisponibleDto>>> GetListaHorariosTutorByCodeTutor(string id)
        {
            var tipoReunionType = typeof(TipoReunion);
            var tutorHorariosDisponibles = await _sesionTutoriaAcademicaContext.TutorHorariosDisponible
                .Where(x => x.IdTutor == id && x.Activo == true)
                .Select(x => new ResponseTutorHorarioDisponibleDto
                {
                    IdHorario = x.IdHorario,
                    IdTutor = x.IdTutor,
                    Hora = x.Hora,
                    Duracion = x.Duracion,
                    Dia = x.Dia,
                    Tipo = Enum.GetName(tipoReunionType, x.Tipo)!,
                    Activo = x.Activo

                }).ToListAsync();

            if (tutorHorariosDisponibles.Count == 0)
                return NotFound();
            return Ok(tutorHorariosDisponibles);
        }
        //-------------------------------------------------------------------------------
        //  Get Tutor Horario Disponible by Code Tutor, fecha y hora
        //-------------------------------------------------------------------------------

        [HttpGet("HorarioByCodeTutorFechaHora")]
        public async Task<ActionResult<ResponseTutorHorarioDisponibleDto>> GetHorarioByCodeTutorDiaHora(string idTutor, string Dia, string Hora)
        {
            // Convierte la cadena Hora a TimeOnly
            TimeOnly horaTimeOnly = TimeOnly.Parse(Hora);

            var tipoReunionType = typeof(TipoReunion);
            var tutorHorarioDisponible = await _sesionTutoriaAcademicaContext.TutorHorariosDisponible
                .Where(x => x.IdTutor == idTutor && x.Dia == Dia && x.Hora == horaTimeOnly && x.Activo == true)
                .Select(x => new ResponseTutorHorarioDisponibleDto
                {
                    IdHorario = x.IdHorario,
                    IdTutor = x.IdTutor,
                    Hora = x.Hora,
                    Duracion = x.Duracion,
                    Dia = x.Dia,
                    Tipo = Enum.GetName(tipoReunionType, x.Tipo)!,
                    Activo = x.Activo

                }).FirstOrDefaultAsync();

            if (tutorHorarioDisponible == null)
                return NotFound();
            return Ok(tutorHorarioDisponible);
        }
        //-------------------------------------------------------------------------------
        //  Desactivar horarios disponibles activos
        //-------------------------------------------------------------------------------
        [HttpPut("DesactivarHorarios")]
        public async Task<IActionResult> PutDesactivarHoraiosDisponiblesActivos()
        {
            
            var horariosActivos = await _sesionTutoriaAcademicaContext.TutorHorariosDisponible
                                            .Where(x => x.Activo == true)
                                            .ToListAsync();
            if (horariosActivos.Count == 0)
                return NotFound();

            //Actualizar activo por inactivo
            horariosActivos.ForEach(x => x.Activo = false);

            //Guardar cambios
            await _sesionTutoriaAcademicaContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Los registros activos fueron desactivados correctamente."
            });
        }


        
    }
}
