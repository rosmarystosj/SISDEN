using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SISDEN.DTOS;
using SISDEN.Models;
using SISDEN.Services;

namespace SISDEN.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly SisdemContext _context;
        private readonly IServicioEmail _emailService;


        public UsuariosController(SisdemContext context, IServicioEmail emailService)
        {
            _context = context;
            _emailService = emailService;   
        }

        [HttpPost("api/Registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroModelo registroModelo, int usurol, string via)
        {
            if (_context.Usuarios.Any(u => u.Usuidentificacion == registroModelo.Usuidentificacion))
            {
                return BadRequest("Este usuario ya está registrado");
            }

            var usuario = new Usuario
            {
                Usunombre = registroModelo.Usunombre,
                Usuapellido = registroModelo.Usuapellido,
                Usuemail = registroModelo.Usuemail,
                Usuidentificacion = registroModelo.Usuidentificacion,
                Usuverificacion = Guid.NewGuid().ToString(),
                Usucontraseña = registroModelo.Usucontraseña,
                Usurol = usurol,
                Usuentidad = registroModelo.Usuentidad,
                Usustatus = registroModelo.Usustatus,
                Usutelefono = registroModelo.Usutelefono,
                Usutelefono2 = registroModelo.Usutelefono2,
            };

            if (usurol == 1 && via == "whatsapp")
            {
                usuario.Usucontraseña = registroModelo.Usuidentificacion;
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();


            var verificationLink = Url.Action("VerifyEmail", "Usuarios", new { token = usuario.Usuverificacion }, Request.Scheme);

            var subject = "Confirmación de registro";
            var message = $"<h1>Bienvenido {usuario.Usunombre} {usuario.Usuapellido}, </h1><p>Gracias por registrarte en nuestra aplicación de registro y gestion de denucnas contra el maltrato animal.</p>" +
                          $"<p>Por favor, verifica tu cuenta haciendo clic en el siguiente enlace: <a href='{verificationLink}'>Verificar cuenta</a></p>";

            await _emailService.SendEmailAsync(usuario.Usuemail, subject, message);

            return Ok(new { Link = verificationLink, Data = registroModelo });
        }


        public async Task<IActionResult> VerifyEmail(string token)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Usuverificacion == token);

            if (usuario == null)
            {
                return BadRequest("El token es invalido");
            }
            usuario.Usustatus = "Activo";

            await _context.SaveChangesAsync();

            return Ok("Cuenta verificada");
        }
    }
}
