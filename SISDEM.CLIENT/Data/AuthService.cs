using SISDEN.DTOS;
using System.Text.Json;

namespace SISDEM.CLIENT.Data

{

    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> ValidarEntidad(RegistroModelo registroModelo)
        {
            var response = await _httpClient.PostAsJsonAsync("api/validarEntidad", registroModelo);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Content: {responseContent}"); // Agrega esta línea para depuración

                var result = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
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
        public async Task<bool> RegistroEntidad(RegistroModelo registroModelo)
        {
            var response = await _httpClient.PostAsJsonAsync("api/registroEntidad", registroModelo);

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

    }

   
}
