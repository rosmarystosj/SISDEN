using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Macs;
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
        private readonly IConfiguration _configuration;


        public UsuariosController(SisdemContext context, IRegistrarDenuncia registrarDenuncia,  IConfiguration configuration,  IServicioEmail servicioEmail)

        {
            _context = context;
            _registrarDenuncia = registrarDenuncia;
            _configuration = configuration;
            _emailService = servicioEmail;


        }
  

        [HttpGet("api/ObtenerUsuarios")]
        public async Task<ActionResult<IEnumerable<VistaUsuario>>> GetUsuarios()
        {

            return await _context.VistaUsuarios.ToListAsync();
        }

        [HttpGet("api/ObtenerUsuario/{correo}")]
        public async Task<ActionResult<Usuario>> GetUsuario(string correo)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Usuemail == correo);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(new { usuario.Usuentidad});
        }

        [HttpGet("api/ObtenerUsuarioAll/{correo}")]
        public async Task<ActionResult<VistaUsuario>> GetUsuarioAll(string correo)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Usuemail == correo);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }
    
        [HttpPost("api/loginDenunciante")]
        public async Task<IActionResult> LoginDenunciante([FromBody] LoginModel loginModel)
        {
            var usuarioLogin = await _context.Usuarios.FirstOrDefaultAsync(u => u.Usuidentificacion == loginModel.Usuidentificacion && u.Usustatus == "Verificado");
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
            
            var usuarioLogin = await _context.Usuarios.FirstOrDefaultAsync(u => u.Usuemail == loginModel.Usuemail && u.Usustatus == "Verificado");
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
            string errorMessage = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    errorMessage = "Datos inválido.";
                    return BadRequest(new { Error = errorMessage });
                }

                var email = await _context.Usuarios.FirstOrDefaultAsync(u => u.Usuemail == registroModelo.Usuemail);
                if (email != null)
                {
                    errorMessage = "Este correo electrónico ya está asociado a una cuenta.";
                    return BadRequest(new { Error = errorMessage });
                }

                var verificacionCedula = VerificarCedula(registroModelo.Usuidentificacion);
                if (!verificacionCedula)
                {
                    errorMessage = "Cédula no válida.";
                    return BadRequest(new { Error = errorMessage });
                }

                if (_context.Usuarios.Any(u => u.Usuidentificacion == registroModelo.Usuidentificacion))
                {
                    errorMessage = "Esta cédula ya está registrada.";
                    return BadRequest(new { Error = errorMessage });
                }

                var passwordHasher = new PasswordHasher<Usuario>();
                var random = new Random();
                var verificationCode = random.Next(100000, 999999).ToString();

                var usuario = new Usuario
                {
                    Usunombre = registroModelo.Usunombre,
                    Usuapellido = registroModelo.Usuapellido,
                    Usuemail = registroModelo.Usuemail,
                    Usuidentificacion = registroModelo.Usuidentificacion,
                    Usuverificacion = verificationCode,
                    VerificationExpiry = DateTime.UtcNow.AddMinutes(15),
                    Usucontraseña = passwordHasher.HashPassword(null, registroModelo.Usucontraseña),
                    Usurol = registroModelo.Usurol,
                    Usustatus = "No verificado",
                    Usutelefono = registroModelo.Usutelefono,
                    Usutelefono2 = registroModelo.Usutelefono2,
                };

                if (via == "whatsapp")
                {
                    usuario.Usucontraseña = passwordHasher.HashPassword(null, registroModelo.Usuidentificacion);
                    usuario.Usustatus = "Verificado";

                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();

                    var subject = "Confirmación de registro";
                    var message = $"<h1>Bienvenido {usuario.Usunombre} {usuario.Usuapellido}, </h1>" +
                                  $"<p>Gracias por registrarte en nuestra aplicación de registro y gestión de denuncias contra el maltrato animal.</p>" +
                                  $"<p>Por favor, verifica tu cuenta ingresando el siguiente código de verificación:</p>" +
                                  $"<h2><strong>{verificationCode}</strong></h2>";

                    try
                    {
                        await _emailService.SendEmailAsync(usuario.Usuemail, subject, message);
                    }
                    catch (Exception ex)
                    {
                        errorMessage = "Error al enviar el correo electrónico de verificación.";
                        return StatusCode(500, new { Error = errorMessage });
                    }
                }

                return Ok(new { Data = registroModelo});
            }
            catch (ApplicationException ex)
            {
                errorMessage = ex.Message;
                return StatusCode(500, new { Error = errorMessage });
            }
            catch (Exception ex)
            {
                errorMessage = $"Ocurrió un error inesperado: {ex.Message}";
                return StatusCode(500, new { Error = errorMessage });
            }
        }



        [HttpPost("api/registroEntidad")]
        public async Task<IActionResult> RegistroEntidad([FromBody] EntidadModel registroModelo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (_context.Usuarios.Any(u => u.Usuentidad == registroModelo.Usuentidad))
            {
                return BadRequest("Datos inválidos");
            }

            var passwordHasher = new PasswordHasher<Usuario>();
            var random = new Random();
            var verificationCode = random.Next(100000, 999999).ToString();

            var usuario = new Usuario
            {
                Usuverificacion = verificationCode,
                Usucontraseña = passwordHasher.HashPassword(null, registroModelo.Usucontraseña),
                Usurol = registroModelo.Usurol,
                Usuentidad = registroModelo.Usuentidad,
                Usustatus = "No verificado",
                VerificationExpiry = DateTime.UtcNow.AddMinutes(15),
                Usutelefono = registroModelo.Usutelefono,
            };

            var mail = await _context.Entidadautorizada
                .Where(ea => ea.Identidadaut == registroModelo.Usuentidad)
                .Select(ea => ea.EntCorreo).FirstOrDefaultAsync();

             usuario.Usuemail = mail;

            var subject = "Confirmación de registro";
            var message = $"<h1>Bienvenido, </h1><p>Gracias por registrarte en nuestra aplicación de registro y gestion de denuncias contra el maltrato animal.</p>" +
                  $"<p>Por favor, verifica tu cuenta como entidad ingresando el siguiente código de verificación: </p>" +
                  $"<h2> <strong>{verificationCode}</strong></h2>";

            await _emailService.SendEmailAsync(mail, subject, message);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { Usuemail = usuario.Usuemail });

        }

        [HttpPost("api/verifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerificarEmail verificarEmail)
        {
            if (verificarEmail == null || string.IsNullOrEmpty(verificarEmail.Email) || string.IsNullOrEmpty(verificarEmail.Codigo))
            {
                return BadRequest("Datos inválidos.");
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Usuemail == verificarEmail.Email);

            if (usuario == null)
            {
                return BadRequest("Usuario no encontrado.");
            }

            if (usuario.Usuverificacion == verificarEmail.Codigo && usuario.VerificationExpiry > DateTime.UtcNow)
            {
                usuario.Usuverificacion = null; 
                usuario.VerificationExpiry = null;
                usuario.Usustatus = "Verificado"; 
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();

                return Ok();
            }

            return BadRequest("Codigo invalido");
        }

        [HttpPost("api/olvidarContra")]
        public async Task<IActionResult> OlvidarContra([FromBody] OlvidarContraseñaDTO olvidarContraseñaDTO)
        {
            if (olvidarContraseñaDTO == null || string.IsNullOrEmpty(olvidarContraseñaDTO.Email))
            {
                return BadRequest("Datos inválidos.");
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Usuemail == olvidarContraseñaDTO.Email);

            if (usuario == null)
            {
                return BadRequest("Usuario no encontrado.");
            }

            var random = new Random();
            var verificationCode = random.Next(100000, 999999).ToString();

            usuario.Usuverificacion = verificationCode;
            usuario.VerificationExpiry = DateTime.UtcNow.AddMinutes(15);

            return Ok(new {usuario });

        }

        [HttpPost("api/ResetearContra")]
        public async Task<IActionResult> ChangePassword([FromBody] ResetearContraseña request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (request.Password!= request.ConfirmPassword)
            {
                return BadRequest("La nueva contraseña y la confirmación no coinciden.");
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Usuemail == request.correo);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            var passwordHasher = new PasswordHasher<Usuario>();

            usuario.Usucontraseña = passwordHasher.HashPassword(usuario, request.correo);
            _context.Usuarios.Update(usuario);

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Contraseña cambiada exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
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
            string plataformaWeb = "http://sisdemfe-dyebf9b7fwc6bygf.eastus-01.azurewebsites.net";
            string enlaceChatBot = "https://wa.me/18296952218?text=Hola";

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
       
        [HttpPost("api/contacto")]
        public async Task <IActionResult> PostContacto([FromBody] ContactoDTO contactoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var subject = "¡Mensaje nuevo del centro de contacto!";

           string message = $"<p>{contactoDTO.UsunombreCompleto} se ha puesto en contacto contigo y ha dejado el siguiente mensaje:</p>" +

           $"<p>{contactoDTO.Mensaje}</p>" +

           $"<p>Te puedes poner en contacto vía este correo electronico {contactoDTO.Usuemail} o a este número {contactoDTO.Usutelefono}</p>";

            await _emailService.SendContactEmailAsync(contactoDTO.Usuemail, contactoDTO.UsunombreCompleto, subject, message);


            return Ok("Mensaje eviado");

        }

        [HttpPost("api/cambiarContra")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return BadRequest("La nueva contraseña y la confirmación no coinciden.");
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Idusuario == request.UserId);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            var passwordHasher = new PasswordHasher<Usuario>();
            var verificationResult = passwordHasher.VerifyHashedPassword(usuario, usuario.Usucontraseña, request.CurrentPassword);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized("La contraseña actual es incorrecta.");
            }

            usuario.Usucontraseña = passwordHasher.HashPassword(usuario, request.NewPassword);
            _context.Usuarios.Update(usuario);

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Contraseña cambiada exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("api/ObtenerMensajeCorreoEntidad")]
        public async Task<IActionResult> MensajeFinalCorreo(int denunciaid)
        {
            string plataformaWeb = "http://sisdemfe-dyebf9b7fwc6bygf.eastus-01.azurewebsites.net";
            string enlaceChatBot = "https://wa.me/18296952218?text=Hola";

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


            var entidad = await _context.Denuncia
            .Where(lv => lv.Iddenuncia == denunciaid)
            .Select(lv => new {
                entidadid = lv.Denentidadid,
                fecha = lv.Denfechacreacion,
                descripcion = lv.Dendescripcion,
                ubicacion = lv.Denubicacion,
                titulo = lv.Dentitulo

            }).FirstOrDefaultAsync();

            var mail = await _context.Entidadautorizada
                .Where(u => u.Identidadaut == entidad.entidadid)
                 .Select(u => new
                 {
                     Correo = u.EntCorreo,
                     Nombre = u.Entautorizadadescp
                 })
    .FirstOrDefaultAsync();



            var subject = "Notificación de Nueva Denuncia Registrada";

            string message = $"<h3>Estimado/a {mail.Nombre}</h3>" +

            $"<p>Espero que este mensaje le encuentre bien.</p>" +

            $"<p>{htmlContent}</p>" +

            $"<p>Le informamos que se ha registrado una nueva denuncia en el sistema que está relacionada con su entidad. A continuación, encontrará los detalles relevantes de la denuncia:</p>" +

            $"<p><strong>Titulo de la denuncia:</strong>{entidad.titulo}</p>" +
            $"<p><strong>Fecha de registro:</strong> {entidad.fecha}</p>" +
            $"<p><strong>Ubicacion:</strong> {entidad.ubicacion} </p>" +
            $"<p><strong>Descripcion:</strong> {entidad.descripcion} </p>" +


           $"<p><strong> Para más detalles, puede acceder y registrase al sistema a través del siguiente enlace: <strong> {plataformaWeb}</p>" +
           $"<p>Le solicitamos que revise la denuncia y tome las acciones necesarias de acuerdo con los procedimientos establecidos. Si tiene alguna pregunta o necesita más información, no dude en contactarnos.</p>" +


            $"<p>Atentamente,</p>" +
            $"<p>El equipo de soporte de denuncias.</p>";

            await _emailService.SendEmailAsync(mail.Correo, subject, message);
            return Ok("Mensaje eviado");

        }
        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(u => u.Idusuario == id);
        }
    }
}
