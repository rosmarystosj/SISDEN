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
            try
            {
                var response = await _httpClient.GetAsync($"api/Obtenerdetalle/{id}");
                response.EnsureSuccessStatusCode(); // Throws an exception if the HTTP response status is an error code.
                return await response.Content.ReadFromJsonAsync<VistaDenuncia>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP error while fetching denuncia by id: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error while fetching denuncia by id: {ex.Message}");
                return null;
            }
        }
        public async Task<DenunciasPorEstadoDTO> GetTotalDenunciasPorEstadoAsync(int? entidadid)
        {
            var response = await _httpClient.GetFromJsonAsync<DenunciasPorEstadoDTO>($"api/TotalDenunciasPorEstado/{entidadid}");
            return response;
        }

        public async Task<DenunciasPorEstadoDTO> GetTotalDenunciasPorEstadoAsyncU(int userid)
        {
            var response = await _httpClient.GetFromJsonAsync<DenunciasPorEstadoDTO>($"api/TotalDenunciasPorEstadoU/{userid}");
            return response;
        }
    }
}
