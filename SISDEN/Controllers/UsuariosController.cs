using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
        private readonly IRegistrarDenuncia _registrarDenuncia;

        public UsuariosController(SisdemContext context, IServicioEmail emailService, IRegistrarDenuncia registrarDenuncia)
        {
            _context = context;
            _emailService = emailService;
            _registrarDenuncia = registrarDenuncia;
        }

        [HttpGet("api/ObtenerUsuarios")]
        public async Task<ActionResult<IEnumerable<VistaUsuario>>> GetUsuarios()
        {

            return await _context.VistaUsuarios.ToListAsync();
        }

        [HttpGet("api/ObtenerUsuario/{id}")]
        public async Task<ActionResult<VistaUsuario>> GetUsuario(int id)
        {
            var usuario = await _context.VistaUsuarios.FirstOrDefaultAsync(u => u.Idusuario == id);
            if (usuario == null) 
            { 
               return NotFound();
            }
           return usuario;
        }
        [HttpPost("api/loginDenunciante")]
        public async Task<IActionResult> LoginDenunciante([FromBody] LoginModel loginModel)

        {
           
            var usuarioLogin = await _context.Usuarios.FirstOrDefaultAsync(u => u.Usuidentificacion == loginModel.Usuidentificacion);
            if (usuarioLogin == null)
            {
                return Unauthorized("Datos invalidos.");
            }
            var passwordHasher = new PasswordHasher<Usuario>();
                var result = passwordHasher.VerifyHashedPassword(usuarioLogin, usuarioLogin.Usucontraseña, loginModel.Usucontraseña);

                if (result == PasswordVerificationResult.Failed)
                {
                    return Unauthorized("Datos invalidos.");
                }

                return Ok("Login exitoso.");
            
        }
        [HttpPost("api/loginEntidad")]
        public async Task<IActionResult> LoginEntidad([FromBody] LoginModel loginModel)
        {
            
            var usuarioLogin = await _context.Usuarios.FirstOrDefaultAsync(u => u.Usuemail == loginModel.Usuemail);
            if (usuarioLogin == null)
            {
                return Unauthorized("Datos invalidos.");
            }
            var passwordHasher = new PasswordHasher<Usuario>();
            var result = passwordHasher.VerifyHashedPassword(usuarioLogin, usuarioLogin.Usucontraseña, loginModel.Usucontraseña);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Datos invalidos.");
            }

            return Ok("Login exitoso.");

        }

        [HttpPost("api/registroDenunciante")]
        public async Task<IActionResult> RegistroDenunciante([FromBody] RegistroModelo registroModelo, string via)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var passwordHasher = new PasswordHasher<Usuario>();

            var usuario = new Usuario
            {
                Usunombre = registroModelo.Usunombre,
                Usuapellido = registroModelo.Usuapellido,
                Usuemail = registroModelo.Usuemail,
                Usuidentificacion= registroModelo.Usuidentificacion,
                Usuverificacion = Guid.NewGuid().ToString(),
                Usucontraseña =  passwordHasher.HashPassword(null, registroModelo.Usucontraseña),
                Usurol = 1,
                Usustatus = registroModelo.Usustatus,
                Usutelefono = registroModelo.Usutelefono,
                Usutelefono2 = registroModelo.Usutelefono2,
            };


            if (via == "whatsapp")
            {
                usuario.Usucontraseña = passwordHasher.HashPassword(null, registroModelo.Usuidentificacion);
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
        [HttpPost("api/validarEntidad")]
        public async Task<IActionResult> ValidarEntidad([FromBody] EntidadModel registroModelo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (_context.Usuarios.Any(u => u.Usuentidad == registroModelo.Usuentidad))
            {
                return BadRequest("Datos inválidos");
            }
           
            var mail = await _context.Entidadautorizada
                .Where(ea => ea.Identidadaut == registroModelo.Usuentidad)
                .Select(ea => ea.EntCorreo).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(mail))
            {
                return BadRequest("Correo no encontrado para la entidad");
            }

            return Ok(new { Usuemail = mail });
           

            
        }
        [HttpPost("api/validarDenunciante")]
        public async Task<IActionResult> ValidarDenunciante([FromBody] RegistroModelo registroModelo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool esCedula = Regex.IsMatch(registroModelo.Usuidentificacion, @"^\d{11}$");
            if (!esCedula)
            {
                return BadRequest("La cedula solo debe contener 11 numeros.");
            }
            var verificacionCedula = VerificarCedula(registroModelo.Usuidentificacion);
            if (!verificacionCedula)
            {
                return BadRequest("Cédula no válida");
            }
            if (_context.Usuarios.Any(u => u.Usuidentificacion == registroModelo.Usuidentificacion))
            {
                return BadRequest("Datos invalidos");
            }

            return Ok();
        }

        [HttpPost("api/registroEntidad")]
        public async Task<IActionResult> RegistroEntidad([FromBody] EntidadModel registroModelo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var passwordHasher = new PasswordHasher<Usuario>();

            var usuario = new Usuario
            {
                Usuverificacion = Guid.NewGuid().ToString(),
                Usucontraseña = passwordHasher.HashPassword(null, registroModelo.Usucontraseña),
                Usurol = 2,
                Usuentidad = registroModelo.Usuentidad,
                Usustatus = registroModelo.Usustatus,
                Usutelefono = registroModelo.Usutelefono,
            };

            var mail = await _context.Entidadautorizada
                .Where(ea => ea.Identidadaut == registroModelo.Usuentidad)
                .Select(ea => ea.EntCorreo).FirstOrDefaultAsync();

             usuario.Usuemail = mail;


            var verificationLink = Url.Action("VerifyEmail", "Usuarios", new { token = usuario.Usuverificacion }, Request.Scheme);

            var subject = "Confirmación de registro";
            var message = $"<h1>Bienvenido, </h1><p>Gracias por registrarte en nuestra aplicación de registro y gestion de denucnas contra el maltrato animal.</p>" +
                          $"<p>Por favor, verifica tu cuenta como entidad haciendo clic en el siguiente enlace: <a href='{verificationLink}'>Verificar cuenta</a></p>";

            await _emailService.SendEmailAsync(mail, subject, message);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { Link = verificationLink, Usuemail = usuario.Usuemail });

        }

        [HttpPut("api/EditarUsuario")]
        public async Task<IActionResult> PutUsuario([FromBody]  int id, RegistroModelo registroModelo)
        {
            if (registroModelo == null)
            {
                return BadRequest("usuario no Valido");
            }
            try
            {
                var passwordHasher = new PasswordHasher<Usuario>();
                var usuario = _context.Usuarios.FirstOrDefault(u => u.Idusuario == id);
                if (usuario == null)
                {
                    return NotFound("Usuario no encontrado");  
                }
                usuario.Usunombre = registroModelo.Usunombre;
                usuario.Usuapellido = registroModelo.Usuapellido;
                usuario.Usuemail = registroModelo.Usuemail;
                usuario.Usuidentificacion = registroModelo.Usuidentificacion;
                usuario.Usucontraseña = passwordHasher.HashPassword(null, registroModelo.Usucontraseña);
                usuario.Usutelefono = registroModelo.Usutelefono;
                usuario.Usutelefono2 = registroModelo.Usutelefono2;

                _context.Entry(usuario).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(usuario);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (UsuarioExists(id))
                {
                    return NotFound("Usuario no encontrado");
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

        [HttpDelete("api/EliminarUsuario/{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Idusuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();

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

        public bool VerificarCedula(string cedula)
        {

            var verificarCedula = new VerificarCedulaDTO();

            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0 || i == 0)
                {
                    verificarCedula.multiplo.Add("1");
                }
                else
                {
                    verificarCedula.multiplo.Add("2");
                }
            }
          

            verificarCedula.suma = 0;
            verificarCedula.longitud = verificarCedula.multiplo.Count;
            foreach (char c in cedula)
            {
                if (cedula != "" && verificarCedula.word <= 9)
                {
                    verificarCedula.producto = Convert.ToInt16(c - 48) * Convert.ToInt16(verificarCedula.multiplo[verificarCedula.word]);
                    verificarCedula.suma += (verificarCedula.producto / 10) + (verificarCedula.producto % 10);
                }
                verificarCedula.word++;
            }
            verificarCedula.entero = (verificarCedula.suma / 10) * 10;
            if (verificarCedula.entero < verificarCedula.suma)
            {
                verificarCedula.entero += 10;
            }
            verificarCedula.digVerificador = Convert.ToString(verificarCedula.entero - verificarCedula.suma);
            if (verificarCedula.digVerificador == cedula.Substring(10, 1))
            {
                verificarCedula.result = true;
            }
            else
            {
                verificarCedula.result = (false);
            }
            verificarCedula.word = 0;



            return verificarCedula.result;


        }
        [HttpGet("api/ObtenerMensaje")]
        public async Task<IActionResult> MensajeFinal(string cedula)
        {
            string plataformaWeb = "";
            string enlaceChatBot = "";

            string mensaje = $@"
                ¡Gracias por usar nuestro servicio de registro de denuncias! Si conoces algún caso, no dudes en contactarnos; tu versión es importante.
                Recuerda que puedes acceder a este chatbot a través del siguiente enlace: {enlaceChatBot}

                Para dar seguimiento a la denuncia que acabas de realizar, puedes acceder al siguiente enlace e iniciar sesión con las credenciales que te hemos proporcionado:

                Credenciales:
                Usuario: {cedula}
                Contraseña: {cedula}
                
                Enlace de la plataforma: {plataformaWeb}";

            return Ok(mensaje);
        }

        [HttpGet("api/ObtenerMensajeCorreo")]
        public async Task<IActionResult> MensajeFinalCorreo(string cedula, int denunciaid, string email)
        {
            string plataformaWeb = "";
            string enlaceChatBot = "";

            var leyes = await _context.VistaViolaciones
                .Where(lv => lv.Iddenuncia == denunciaid)
                .Select(lv => new
                {
                    lv.Artnombre,
                    lv.Artdescripcion,
                    lv.Puntoartnumero,
                    lv.Puntoartdescripcion
                }).ToListAsync();


            var listaArt = leyes.Select(lv =>
            {
                if (lv.Puntoartnumero != null && lv.Puntoartdescripcion != null)
                {
                    return $"- {lv.Artnombre}  Punto {lv.Puntoartnumero}: {lv.Puntoartdescripcion}";
                }
                else
                {
                    return $"- {lv.Artnombre} {lv.Artdescripcion} ";

                }
            });

            string htmlContent = string.Join("", listaArt.Select(art => $"<p>{art}</p>"));

            var subject = "¡Denuncia registrada!";

            string message = $"<h3>¡Gracias por usar nuestro servicio de registro de denuncias!</h3>" +

            $"<p>Nos complace informarle que su denuncia se ha registrado correctamente. Según las respuestas que nos proporcionó en el chatbot, <strong>podemos determinar que se han violado los siguientes artículos</strong> de la Ley de Protección Animal y Tenencia Responsable, No. 248-12:</p>" +

            $"<p>{htmlContent}</p>" +

            $"<p>Para dar seguimiento a la denuncia que acaba de realizar,<strong> puede acceder al siguiente enlace e iniciar sesión con las credenciales que le hemos proporcionado:</strong></p>" +

            $"<p><strong>Credenciales:</strong></p>" +
            $"<p><strong>Usuario:</strong> {cedula}</p>" +
            $"<p><strong>Contraseña:</strong> {cedula} </p>" +

           $"<p><strong> Enlace de la plataforma:<strong> {plataformaWeb}</p>" +
           $"<p>Recuerde que puede <strong>acceder a este chatbot</strong> a través del siguiente enlace: {enlaceChatBot}</p>" +


            $"<p>Atentamente,</p>" +
            $"<p>El equipo de soporte de denuncias.</p>";

            await _emailService.SendEmailAsync(email, subject, message);

            return Ok("Mensaje eviado");
            
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(u => u.Idusuario == id);
        }
    }
}
