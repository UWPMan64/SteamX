using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SteamX.Services
{
    public class NetworkService
    {
        private static readonly HttpClient Client = new HttpClient();

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using (var response = await Client.GetAsync(
                    "https://store.steampowered.com/"))
                {
                    return response.IsSuccessStatusCode;
                }
            }
            catch (HttpRequestException)
            {
                return false;
            }
            catch (TaskCanceledException)
            {
                return false;
            }
        }
    }
}
