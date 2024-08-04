using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SISDEN.Models;
using SISDEN.DTOS;
using System.Data.SqlClient;
using SISDEN.Services;
using Microsoft.Data.SqlClient;


namespace SISDEN.Controllers
{
    public class DenunciasController : Controller
    {
        private readonly SisdemContext _context;
        private readonly IRegistrarDenuncia _registrarDenuncia;
        private readonly NotificationService _notificationService;


        public DenunciasController(SisdemContext context, IRegistrarDenuncia registrarDenuncia, NotificationService notificationService)
        {
            _context = context;
            _registrarDenuncia = registrarDenuncia;
            _notificationService = notificationService;


        }
        [HttpGet("api/ObtenerIDDenuncia/{sesionid}")]
        public async Task<IActionResult> GetIDDenuncias(string sesionid)
        {
            var denunciaid = await _registrarDenuncia.RegistrarDenunciaAsync(sesionid);
            return Ok(new { denunciaid });
        }


        [HttpGet("api/ObtenerDenuncias/entidad/{entidadid}")]
        public async Task<IActionResult> GetIDDenunciasentidad(int entidadid)
        {
            if (entidadid == 13) {
                var todasDenuncias = await _context.VistaDenuncias.ToListAsync();
                return Ok(new { denuncias = todasDenuncias });
            }
            else {
                var entidadIdParameter = new SqlParameter("@EntidadId", entidadid);

                var denuncias = await _context.VistaDenuncias
                    .FromSqlRaw("EXEC GetDenunciasByEntidadId @EntidadId", entidadIdParameter)
                    .ToListAsync();

                return Ok(new { denuncias });
            }
        }
        [HttpGet("api/ObtenerDenuncias")]
        public async Task<ActionResult<IEnumerable<VistaDenuncia>>> GetDenuncias()
        {

             return await _context.VistaDenuncias.ToListAsync();
        }

        [HttpGet("api/ObtenerDenuncia/{id}")]
        public async Task<ActionResult<VistaDenuncia>> GetDenuncia(int id)
        {
            var denuncia = await _context.VistaDenuncias.FirstOrDefaultAsync(d => d.Iddenuncia == id);
            if (denuncia == null)
            {
                return NotFound();

            }
            return denuncia;
        }
        [HttpPost("api/RegistarDenuncias")]
        public async Task<ActionResult> PostDenuncia([FromBody] DenunciasDTO denunciaDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var denuncia = new Denuncium
                {  
                    Dentitulo = denunciaDTO.Dentitulo,
                    Dendescripcion = denunciaDTO.Dendescripcion,
                    Denanimal = denunciaDTO.Denanimal,
                    Denfechacreacion = DateTime.Now,
                    Denfechacierre = denunciaDTO.Denfechacierre,
                    Denevidenciaadjunta = denunciaDTO.Denevidenciaadjunta,
                    Denservicio = denunciaDTO.Denservicio,           
                    Densalarios = denunciaDTO.Densalarios,
                    Denprision = denunciaDTO.Denprision,
                    Dennumsalarios = denunciaDTO.Dennumsalarios,
                    Dennumprision = denunciaDTO.Dennumprision,
                    Dennumserv = denunciaDTO.Dennumserv,
                    Denobservaciones = denunciaDTO.Denobservaciones,
                    DenIdmotivocierre = denunciaDTO.DenIdmotivocierre,
                    Denubicacion = denunciaDTO.DenIdubicacion,
                    DenIdusuario = denunciaDTO.DenIdusuario,
                    DenIdestado = denunciaDTO.DenIdestado,
                    Dencategoria =denunciaDTO.DenCategoria,
                };

                
                _context.Denuncia.Add(denuncia);
                await _context.SaveChangesAsync();

                // Crear y enviar notificación
                var estado = await _context.Estados.FindAsync(denuncia.DenIdestado);
                var mensaje = $"Se ha registrado una nueva denuncia con el estado: {estado.Estdescripcion}";

                if (denuncia.DenIdusuario.HasValue && denuncia.DenIdestado.HasValue)
                {
                    await _notificationService.CreateNotificationAsync(denuncia.DenIdusuario.Value, denuncia.DenIdestado.Value, mensaje);
                }
                else
                {
                    // Manejar el caso en que los valores sean nulos
                    // Podrías lanzar una excepción, registrar un error, etc.
                    // Ejemplo:
                    throw new InvalidOperationException("DenIdusuario o DenIdestado son nulos.");
                }

                return Ok(new { id = denuncia.Iddenuncia, denuncia });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        
        [HttpPut("api/EditarDenuncia/{id}")]
        public async Task<IActionResult> PutDenuncia([FromBody] int id, DenunciasDTO denunciaDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var denuncia = await _context.Denuncia.FirstOrDefaultAsync(d => d.Iddenuncia == id);

                if (denuncia == null)
                {
                   return NotFound("Denuncia no encontrada");  

                }
                if (denunciaDTO.Denanimal == "Ambos")
                {
                    denunciaDTO.Denanimal = "Perros y gatos";
                }
                denuncia.Dendescripcion = denunciaDTO.Dendescripcion;
                denuncia.Denanimal = denunciaDTO.Denanimal;
                denuncia.Denfechacierre = denunciaDTO.Denfechacierre;
                denuncia.Denevidenciaadjunta = denunciaDTO.Denevidenciaadjunta;
                denuncia.Denservicio = denunciaDTO.Denservicio;
                denuncia.Densalarios = denunciaDTO.Densalarios;
                denuncia.Denprision = denunciaDTO.Denprision;
                denuncia.Dennumsalarios = denunciaDTO.Dennumsalarios;
                denuncia.Dennumprision = denunciaDTO.Dennumprision;
                denuncia.Dennumserv = denunciaDTO.Dennumserv;
                denuncia.Denobservaciones = denunciaDTO.Denobservaciones;
                denuncia.DenIdmotivocierre = denunciaDTO.DenIdmotivocierre;
                denuncia.Denubicacion = denunciaDTO.DenIdubicacion;
                denuncia.DenIdusuario = denunciaDTO.DenIdusuario;
                denuncia.DenIdestado = denunciaDTO.DenIdestado;
                denuncia.Dencategoria = denunciaDTO.DenCategoria;


                _context.Entry(denuncia).State = EntityState.Modified;


                await _context.SaveChangesAsync();

                denuncia.Dentitulo = GenerarTitulo(denunciaDTO);
                await _context.SaveChangesAsync();

                var estado = await _context.Estados.FindAsync(denuncia.DenIdestado);
                var mensaje = $"Se ha registrado una nueva denuncia con el estado: {estado.Estdescripcion}";

                if (denuncia.DenIdusuario.HasValue && denuncia.DenIdestado.HasValue)
                {
                    await _notificationService.CreateNotificationAsync(denuncia.DenIdusuario.Value, denuncia.DenIdestado.Value, mensaje);
                }
                else
                {
                    // Manejar el caso en que los valores sean nulos
                    // Podrías lanzar una excepción, registrar un error, etc.
                    // Ejemplo:
                    throw new InvalidOperationException("DenIdusuario o DenIdestado son nulos.");
                }

                return Ok( denuncia );
            }
            catch (DbUpdateConcurrencyException)
            {

                if (DenunciaExists(id))
                {
                    return NotFound("Denuncia no encontrada");
                }
                else 
                {
                    throw;
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("api/EliminarDenuncia/{id}")]
         public async Task<IActionResult> DeleteDenuncia(int id)
        {
            var denuncia = await _context.Denuncia.FirstOrDefaultAsync(d => d.Iddenuncia == id);   
            if (denuncia == null)
            {
                return NotFound();

            }
            _context.Denuncia.Remove(denuncia);
            await _context.SaveChangesAsync();

            return NoContent();

         }


        [HttpPost("api/TomarCategoria")]

        public string GenerarTitulo(DenunciasDTO denunciasDTO)
        {
            var titulo = $" Denuncia sobre {denunciasDTO.DenCategoria} de {denunciasDTO.Denanimal} en {denunciasDTO.DenIdubicacion}";


             return titulo;
        }
        private bool DenunciaExists(int id)
        {
            return _context.Denuncia.Any(e => e.Iddenuncia == id);
        }
    }

}

