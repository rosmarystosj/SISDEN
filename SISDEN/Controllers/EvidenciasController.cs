using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using NuGet.Versioning;
using SISDEN.Models;
using SISDEN.Services;

namespace SISDEN.Controllers
{
    public class EvidenciasController : Controller
    {
        private readonly SisdemContext _context;
        private readonly IRegistrarDenuncia _registrarDenuncia;
        private readonly ISesion _sesion;

        public EvidenciasController(SisdemContext context, IRegistrarDenuncia registrarDenuncia, ISesion sesion)
        {
            _context = context;
            _registrarDenuncia = registrarDenuncia; 
            _sesion = sesion;   
        }
        [HttpGet("api/ObtenerEvidencias")]
        public async Task<ActionResult<IEnumerable<VistaEvidencia>>> GetEvidencia()
        {
            return await _context.VistaEvidencias.ToListAsync();
        }

        [HttpGet("api/ObtenerEvidenicia/{id}")]
        public async Task<ActionResult<VistaEvidencia>> GetEvidencia(int id)
        {
            var denuncia = await _context.VistaEvidencias.FirstOrDefaultAsync(e => e.Idevidencia == id);
            if (denuncia == null)
            {
                return BadRequest("Evidenicia no encontrada");
            }
            return denuncia;
        }

        [HttpPost("api/GuardarArchivo")]
        public async Task<IActionResult> GuardarArchivos(IFormFile file, [FromHeader] string sesionId)
        {
            var userid = await _sesion.ObtenerUserIdAsync(sesionId);
            if (userid == null)
            {
                return Unauthorized("Sesion expirada o invalida");
            }

            GuardarArchivo _guardar = new GuardarArchivo();
            _guardar.Archivo = file;    
            var archivo = _guardar.Archivo;


            if (archivo == null || archivo.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            string uploadsFolder = "";

            if (archivo.FileName.Contains(".mp4"))
            {
              uploadsFolder = Path.Combine("Archivos", "Videos");
              _guardar.ruta = Path.Combine(uploadsFolder, archivo.FileName);
              _guardar.tipoevidencia = 2;
                     
            }
            else if (archivo.FileName.Contains(".jp") || archivo.FileName.Contains(".png") || archivo.FileName.Contains(".bmp") || _guardar.Archivo.FileName.Contains(".webp"))
            {
            uploadsFolder = Path.Combine("Archivos", "Fotos");
            _guardar.ruta = Path.Combine(uploadsFolder, archivo.FileName);
            _guardar.tipoevidencia = 1;

            }
            else if (_guardar.Archivo.FileName.Contains(".docx") || _guardar.Archivo.FileName.Contains(".pdf"))
            {
                uploadsFolder = Path.Combine("Archivos", "Documentos");
            _guardar.ruta = Path.Combine(uploadsFolder, archivo.FileName);
                _guardar.tipoevidencia = 3;
            }
            if 
            (!Directory.Exists(uploadsFolder))
            {
            Directory.CreateDirectory(uploadsFolder);
            }

            try
            {
            using (var stream = new FileStream(_guardar.ruta, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            int denunciaId = await _registrarDenuncia.RegistrarDenunciaAsync(sesionId);
            var evidencia = new Evidencium()
            {
                EvIddenuncia = denunciaId,
                Evurl = _guardar.ruta,
                EvIdtipoevid = _guardar.tipoevidencia,
            };
            _context.Evidencia.Add(evidencia);
            await _context.SaveChangesAsync();

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var fileUrl = $"{baseUrl}/{_guardar.ruta}/{file.FileName}";

                return Ok(new {fileUrl});


            }
            catch (Exception ex)
            {
            return StatusCode(500, $"Ocurrió un error al guardar el archivo: {ex.Message}");

            }
    
        }

    }
}
 