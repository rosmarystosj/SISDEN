﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISDEN.DTOS;
using SISDEN.Models;

namespace SISDEN.Controllers
{
    public class ArticulosController : Controller
    {
        private readonly SisdemContext _context;

        public ArticulosController(SisdemContext context)
        {
            _context = context;
        }

        [HttpGet("api/ObtenerArticulos")]
        public async Task<ActionResult<IEnumerable<VistaArticulo>>> GetArticulos()
        {
            return await _context.VistaArticulos.ToListAsync();
        }
        [HttpGet("api/ObtenerArticulos/{id}")]
        public async Task<ActionResult<VistaArticulo>> GetArticulo(int id)
        {
            var articulo  = await _context.VistaArticulos.FirstOrDefaultAsync(va => va.Idarticulo == id);
            if (articulo == null)
            {
                return BadRequest("Articulo no encontrado");
            }
            return articulo;
        }
    }
}