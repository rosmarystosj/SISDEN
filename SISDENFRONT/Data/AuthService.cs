using SISDEN.DTOS;
using System.Text.Json;

namespace SISDENFRONT.Data
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> RegistroDenunciante(RegistroModelo registroModelo, string via)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/registroDenunciante?via={via}", registroModelo);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<dynamic>();
                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(errorMessage);
            }
        }
        public async Task<string> ValidarEntidad(EntidadModel entidadModelo)
        {
            var response = await _httpClient.PostAsJsonAsync("api/validarEntidad", entidadModelo);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
               
                    var result = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
                    if (result != null && result.ContainsKey("usuemail"))
                    {
                        return result["usuemail"];
                    }
                    else
                    {
                        throw new ApplicationException("Respuesta inesperada del servidor.");
                    }
            
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(errorMessage);
            }
        }
        public async Task<bool> ValidarDenunciante(RegistroModelo registroModelo)
        {
            var response = await _httpClient.PostAsJsonAsync("api/validarDenunciante", registroModelo);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(errorMessage);
            }
        }
        public async Task<bool> RegistroEntidad(EntidadModel entidadModelo)
        {
            var response = await _httpClient.PostAsJsonAsync("api/registroEntidad", entidadModelo);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<dynamic>();
                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(errorMessage);
            }
        }
        public async Task<bool> LoginDenunciante(LoginModel loginModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/loginDenunciante", loginModel);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(errorMessage);
            }
        }
        public async Task<bool> LoginEntidad(LoginModel loginModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/loginEntidad", loginModel);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(errorMessage);
            }
        }
    }
}
