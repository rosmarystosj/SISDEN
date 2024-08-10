using SISDEN.DTOS;
using SISDEN.Models;
using System.Net.Http;

namespace SISDENFRONT.Data
{
    public class DenunciasService
    {
        private HttpClient _httpClient;

        public DenunciasService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<VistaDenuncia>> GetDenunciasByCedulaAsync(string cedula)
        {
            var response = await _httpClient.GetAsync($"api/ObtenerDenuncia/{cedula}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<VistaDenuncia>>();
        }

        public async Task<List<VistaDenuncia>> GetDenunciasByEntidadIdAsync(int entidadid)
        {
            var response = await _httpClient.GetAsync($"api/ObtenerDenuncias/entidad/{entidadid}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<VistaDenuncia>>();
        }

        public async Task<VistaDenuncia> GetDenunciaById(int id)
        {
            return await _httpClient.GetFromJsonAsync<VistaDenuncia>($"api/Obtenerdetalle/{id}");
        }
        
    }
}
