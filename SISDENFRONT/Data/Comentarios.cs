using SISDEN.Models;
using SISDEN.DTOS;


namespace SISDENFRONT.Data
{
    public class Comentarios
    {
        private readonly HttpClient _httpClient;

        public Comentarios(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<VistaComentario>> GetCommentsByDenunciaId(int denunciaId)
        {
            return await _httpClient.GetFromJsonAsync<List<VistaComentario>>($"api/Comertariopordenuncia/{denunciaId}");
        }

        public async Task AddComment(ComentarioDTO comment)
        {
            await _httpClient.PostAsJsonAsync("api/Comentario", comment);
        }
    }

}
