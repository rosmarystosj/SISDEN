using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SISDEN.Models;

namespace SISDEN.Controllers
{
    public class DenunciumsController : Controller
    {
        private readonly SisdemContext _context;

        public DenunciumsController(SisdemContext context)
        {
            _context = context;
        }


      
        [HttpPost("api/RegistarDenuncias")]
        public async Task<ActionResult<Denuncium>> PostDenuncia(Denuncium denuncia)
        {
            if (denuncia == null)
            {
                return BadRequest("Denuncia no válida");
            }

            try
            {
                denuncia.Denfechacreacion = DateTime.Now;
                _context.Denuncia.Add(denuncia);
                await _context.SaveChangesAsync();

                return Ok(new { id = denuncia.Iddenuncia, denuncia });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("api/EditarDenuncia/{id}")]
        public async Task<IActionResult> Denuncia(int id, Denuncium denuncia)
        {
            if (id != denuncia.Iddenuncia)
            {
                return BadRequest("ID de denuncia no coincide");
            }

            if (denuncia == null)
            {
                return BadRequest("Denuncia no válida");
            }

            _context.Entry(denuncia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Denuncia actualizada correctamente", denuncia });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DenunciaExists(id))
                {
                    return NotFound("La denuncia no existe");
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

        private bool DenunciaExists(int id)
        {
            return _context.Denuncia.Any(e => e.Iddenuncia == id);
        }
    }

}

