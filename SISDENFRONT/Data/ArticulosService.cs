using SISDEN.DTOS;
using SISDEN.Models;
using System.Net.Http;


namespace SISDENFRONT.Data
{
    
    public class ArticulosService
    {
        private HttpClient _httpClient;

        public ArticulosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<VistaViolacione>> GetArticuloById(int denunciaid)
        {
            return await _httpClient.GetFromJsonAsync<List<VistaViolacione>>($"api/ObtenerViolacionesPorDenuncia/{denunciaid}");
        }
    }
}
