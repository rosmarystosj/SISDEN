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
    }

}
