using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using SISDEN.Models;

namespace SISDEN.Controllers
{
    public class EvidenciumsController : Controller
    {
        private readonly SisdemContext _context;

        public EvidenciumsController(SisdemContext context)
        {
            _context = context;
        }

        // GET: Evidenciums/Create
        public IActionResult Create()
        {
            return View();
        }


        public async Task<string> SubirStorage(Stream archivo, string nombre)
        {
            string email = "sistemadedenunciaschatbot@gmail.com";
            string clave = "UNPHU17-1018-19-1409";
            string ruta = "gs://sisden-7ed7d.appspot.com";
            string apiKey = "AIzaSyBud3h7zn_0dOso9k9bpCCm-NSzwVRB9Q";

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                ruta,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child("uploads")
                .Child(nombre)
                .PutAsync(archivo, cancellation.Token);

            try
            {
                string link = await task;
                return link;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al subir la imagen", ex);
            }
        }

        private bool EvidenciumExists(int id)
        {
            return (_context.Evidencia?.Any(e => e.Idevidencia == id)).GetValueOrDefault();
        }
    }
}