using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using SISDEN.DTOS;
using System.Text.Json;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace SISDENFRONT.Data
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenProvider _tokenProvider;
        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _jsRuntime;

        public AuthService(HttpClient httpClient, TokenProvider tokenProvider, NavigationManager navigationManager, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
            _navigationManager = navigationManager;
            _jsRuntime = jsRuntime;
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
     
      
        public async Task<string> RegistroEntidad(EntidadModel entidadModelo)
        {
            var response = await _httpClient.PostAsJsonAsync("api/registroEntidad", entidadModelo);

            if (response.IsSuccessStatusCode)
            {

                var responseContent = await response.Content.ReadFromJsonAsync<dynamic>();
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
        public async Task<bool> LoginDenunciante(LoginModel loginModel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/loginDenunciante", loginModel);

            if (response.IsSuccessStatusCode)
            {
                 _navigationManager.NavigateTo("/index", forceLoad: true);

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
                _navigationManager.NavigateTo("/index", forceLoad: true);

                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(errorMessage);
            }
        }

        public async Task<bool> VerificarEmail(VerificarEmail verificarEmail)
        {
            var response = await _httpClient.PostAsJsonAsync("api/verifyEmail", verificarEmail);

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
        public async Task<bool> Contacto(ContactoDTO contactoDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("api/contacto", contactoDTO);

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
        public async Task<bool> EditarUsuario(RegistroModelo registroModelo)
        {
            var response = await _httpClient.PostAsJsonAsync("api/EditarUsuario", registroModelo);

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
