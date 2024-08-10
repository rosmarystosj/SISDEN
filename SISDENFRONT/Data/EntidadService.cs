using Microsoft.AspNetCore.Http.HttpResults;
using SISDEN.DTOS;


namespace SISDENFRONT.Data
{
    public class EntidadService
    {
        private readonly HttpClient _httpClient;

        public EntidadService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<EntidadAutorizadaDTO>> GetEntidadesAutorizadasAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<EntidadAutorizadaDTO>>("api/EntidadAutorizada");
        }

        public async Task<int> GetEntidadIdByEmailAsync(string correo)
        {
            var response = await _httpClient.GetAsync($"api/ObtenerUsuario/{correo}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, int>>();
                if (result != null && result.ContainsKey("usuentidad"))
                {
                    return result["usuentidad"];
                }
            }
            return 0;

        }
    }

}
