using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml.Media.Imaging;
using SteamX.Models;

namespace SteamX.Services
{
    // Uses an undocumented Steam Store endpoint. It may change or be rate-limited.
    public class SteamStoreService
    {
        private static readonly HttpClient Client = new HttpClient();

        public async Task<List<Game>> SearchGamesAsync(string searchTerm)
        {
            var games = new List<Game>();

            if (string.IsNullOrWhiteSpace(searchTerm))
                return games;

            string url =
                "https://store.steampowered.com/api/storesearch/" +
                "?term=" + Uri.EscapeDataString(searchTerm) +
                "&l=english&cc=us";

            using (var response = await Client.GetAsync(new Uri(url)))
            {
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                JsonObject root = JsonObject.Parse(json);

                IJsonValue itemsValue;
                if (!root.TryGetValue("items", out itemsValue) ||
                    itemsValue.ValueType != JsonValueType.Array)
                    return games;

                foreach (IJsonValue value in itemsValue.GetArray())
                {
                    if (value.ValueType != JsonValueType.Object)
                        continue;

                    JsonObject item = value.GetObject();
                    string imageUrl = ReadString(item, "tiny_image");
                    var game = new Game
                    {
                        AppId = ReadInt(item, "id"),
                        Name = ReadString(item, "name"),
                        ImageUrl = imageUrl,
                        Price = ReadPrice(item)
                    };

                    if (!string.IsNullOrWhiteSpace(imageUrl))
                    {
                        try
                        {
                            game.Artwork = new BitmapImage(new Uri(imageUrl));
                        }
                        catch
                        {
                            // Artwork is optional; keep the game result.
                        }
                    }

                    games.Add(game);
                }
            }

            return games;
        }

        private static int ReadInt(JsonObject obj, string key)
        {
            IJsonValue value;
            if (obj.TryGetValue(key, out value) &&
                value.ValueType == JsonValueType.Number)
                return (int)value.GetNumber();

            return 0;
        }

        private static string ReadString(JsonObject obj, string key)
        {
            IJsonValue value;
            if (obj.TryGetValue(key, out value) &&
                value.ValueType == JsonValueType.String)
                return value.GetString();

            return string.Empty;
        }

        private static string ReadPrice(JsonObject item)
        {
            IJsonValue priceValue;
            if (!item.TryGetValue("price", out priceValue) ||
                priceValue.ValueType != JsonValueType.Object)
                return "Price unavailable";

            JsonObject priceObject = priceValue.GetObject();
            string formatted = ReadString(priceObject, "final_formatted");

            return string.IsNullOrWhiteSpace(formatted)
                ? "Price unavailable"
                : formatted;
        }
    }
}
