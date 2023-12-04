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
    public class ConfiguracionAcademicaController : ControllerBase
    {
        private readonly AppDbContext _cargaTutoriaContext;

        public ConfiguracionAcademicaController(AppDbContext appDbContext)
        {
            _cargaTutoriaContext = appDbContext;
        }
        //===============================================================================
        //          PERSON
        //===============================================================================
        //[HttpPost("Agregar")]
        //public async Task<IActionResult> AgregarPersona([FromBody] Person personObj)
        //{
        //    if (personObj == null)
        //    {
        //        return BadRequest();
        //    }
        //    if (await CheckPersonExistAsync(personObj.Code!))
        //    {
        //        return BadRequest(new { Message = "El codigo ya existe!" });
        //    }
        //    //Agregar
        //    await _cargaTutoriaContext.Persons.AddAsync(personObj);
        //    //Guardar
        //    await _cargaTutoriaContext.SaveChangesAsync();

        //    return Ok(new
        //    {
        //        Message = "Persona registrado!"
        //    });

        //}

        //private Task<bool> CheckPersonExistAsync(string code)
        //    => _cargaTutoriaContext.Persons.AnyAsync(p => p.Code == code);



        


        
    }
}
