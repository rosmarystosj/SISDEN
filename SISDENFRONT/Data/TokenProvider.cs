using Blazored.LocalStorage;

namespace SISDENFRONT.Data
{
    public class TokenProvider
    {
        private readonly ILocalStorageService _localStorageService;

        public TokenProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task SetTokenAsync(string token)
        {
            await _localStorageService.SetItemAsync("authToken", token);
        }

        public async Task<string> GetTokenAsync()
        {
            return await _localStorageService.GetItemAsync<string>("authToken");
        }

        public async Task ClearTokenAsync()
        {
            await _localStorageService.RemoveItemAsync("authToken");
        }
    }

}
