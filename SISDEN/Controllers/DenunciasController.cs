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
using System.Data;


namespace SISDEN.Controllers
{
    public class DenunciasController : Controller
    {
        private readonly SisdemContext _context;
        private readonly IRegistrarDenuncia _registrarDenuncia;
        private readonly IServicioEmail _servicioEmail;



        public DenunciasController(SisdemContext context, IRegistrarDenuncia registrarDenuncia, IServicioEmail emailService)
        {
            _context = context;
            _registrarDenuncia = registrarDenuncia;
            _servicioEmail = emailService;



        }
        [HttpGet("api/ObtenerDenunciaSS/{sesionid}")]
        public async Task<IActionResult> GetIDDenuncias(string sesionid)
        {
            var denunciaid = await _registrarDenuncia.RegistrarDenunciaAsync(sesionid);
            return Ok(new { denunciaid });
        }
        [HttpGet("api/Obtenerdetalle/{id}")]
        public async Task<ActionResult<VistaDenuncia>> GetIDDenunciaid(int id)
        {
            var denuncias = await _context.VistaDenuncias.FirstOrDefaultAsync(d => d.Iddenuncia == id);
            if (denuncias == null)
            {
                return BadRequest("Denuncia no encontrada");
            }
            return denuncias;
        }

        [HttpGet("api/ObtenerDenuncias/entidad/{entidadid}")]
        public async Task<ActionResult<IEnumerable<VistaDenuncia>>> GetIDDenunciasentidad(int entidadid)
        {
            if (entidadid == 14) {
                var todasDenuncias = await _context.VistaDenuncias.ToListAsync();
                return Ok(new { denuncias = todasDenuncias });
            }
            else {
                var entidadIdParameter = new SqlParameter("@EntidadId", entidadid);

                var denuncias = await _context.VistaDenuncias
                    .FromSqlRaw("EXEC GetDenunciasByEntidadId @EntidadId", entidadIdParameter)
                    .ToListAsync();

                 foreach (var denuncia in denuncias)
    {
        denuncia.Denentidadid = entidadid;
    }

    await _context.SaveChangesAsync();

                return Ok(denuncias );
            }
        }

      
        [HttpGet("api/ObtenerAllDenuncias")]
        public async Task<ActionResult<IEnumerable<VistaDenuncia>>> GetDenuncias()
        {

             return await _context.VistaDenuncias.ToListAsync();
        }

       [HttpGet("api/ObtenerDenuncia/{cedula}")]
        public async Task<ActionResult<IEnumerable<VistaDenuncia>>> GetDenunciasByCedula(string cedula)
        {
            var denuncia = await _context.VistaDenuncias.Where(d => d.CedulaUsuario == cedula).ToListAsync();
            if (denuncia == null || denuncia.Count == 0)
            {
                return NotFound(new { message = "No se encontraron denuncias." });

            }
            return Ok(denuncia);
        }

       
        [HttpPost("api/RegistarDenuncias")]
        public async Task<ActionResult> PostDenuncia([FromBody] DenunciasDTO denunciaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

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
                denuncia.Denentidadid = denunciaDTO.DenEntidadid;
                   
                _context.Entry(denuncia).State = EntityState.Modified;

                var ubicacionString = denunciaDTO.DenIdubicacion; 
                var entidadIdParam = new SqlParameter("@EntidadId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC AssignEntidadId @UbicacionString, @EntidadId OUTPUT",
                    new SqlParameter("@UbicacionString", ubicacionString),
                    entidadIdParam
                );

                denuncia.Denentidadid = (int)entidadIdParam.Value;

                await _context.SaveChangesAsync();

                denuncia.Dentitulo = GenerarTitulo(denunciaDTO);
                await _context.SaveChangesAsync();

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
            var titulo = $" Denuncia sobre {denunciasDTO.DenCategoria} de {denunciasDTO.Denanimal} ";


             return titulo;
        }

        [HttpGet("api/ObtenerMensajeCorreoEntidad")]
        public async Task<IActionResult> MensajeFinalCorreo(int denunciaid)
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

                await _servicioEmail.SendEmailAsync(mail.Correo, subject, message);
            return Ok("Mensaje eviado");

        }
        private bool DenunciaExists(int id)
        {
            return _context.Denuncia.Any(e => e.Iddenuncia == id);
        }
    }

}

