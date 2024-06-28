using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using NuGet.Versioning;
using SISDEN.Models;

namespace SISDEN.Controllers
{
    public class EvidenciasController : Controller
    {
        private readonly SisdemContext _context;
        public EvidenciasController(SisdemContext context)
        {
            _context = context;

        }


        [HttpPost("api/GuardarArchivo")]
    
        public async Task<IActionResult> GuardarArchivos(IFormFile file)
        {
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

                Denuncium denuncium = new Denuncium();
                

                var evidencia = new Evidencium()
                {
                    EvIddenuncia = denuncium.Iddenuncia,
                    Evurl = _guardar.ruta,
                    EvIdtipoevid = _guardar.tipoevidencia,
                };
                _context.Evidencia.Add(evidencia);
                await _context.SaveChangesAsync();

                var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                var fileUrl = $"{baseUrl}/{_guardar.ruta}/{file.FileName}";

                return Ok(new { fileUrl });

               }
            catch (Exception ex)
              {
                return StatusCode(500, $"Ocurrió un error al guardar el archivo: {ex.Message}");

              }
    
        }
       

    }
}
 