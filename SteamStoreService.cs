using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml.Media.Imaging;
using SteamX.Models;

namespace SteamX.Services
{
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
                            game.Artwork = new BitmapImage(
                                new Uri(imageUrl));
                        }
                        catch
                        {
                        }
                    }

                    games.Add(game);
                }
            }

            return games;
        }

        public async Task<Game> GetGameDetailsAsync(int appId)
        {
            string url =
                "https://store.steampowered.com/api/appdetails" +
                "?appids=" + appId +
                "&l=english&cc=us";

            using (var response = await Client.GetAsync(new Uri(url)))
            {
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                JsonObject root = JsonObject.Parse(json);

                IJsonValue appValue;

                if (!root.TryGetValue(appId.ToString(), out appValue) ||
                    appValue.ValueType != JsonValueType.Object)
                    return null;

                JsonObject appObject = appValue.GetObject();

                IJsonValue successValue;

                if (!appObject.TryGetValue("success", out successValue) ||
                    successValue.ValueType != JsonValueType.Boolean ||
                    !successValue.GetBoolean())
                    return null;

                IJsonValue dataValue;

                if (!appObject.TryGetValue("data", out dataValue) ||
                    dataValue.ValueType != JsonValueType.Object)
                    return null;

                JsonObject data = dataValue.GetObject();

                var game = new Game
                {
                    AppId = appId,
                    Name = ReadString(data, "name"),
                    Description = ReadString(data, "short_description"),
                    Developers = ReadStringArray(data, "developers"),
                    Publishers = ReadStringArray(data, "publishers"),
                    ReleaseDate = ReadReleaseDate(data),
                    HeaderImageUrl = ReadString(data, "header_image"),
                    Price = ReadDetailedPrice(data)
                };

                if (!string.IsNullOrWhiteSpace(game.HeaderImageUrl))
                {
                    game.HeaderArtwork = new BitmapImage(
                        new Uri(game.HeaderImageUrl));
                }

                LoadScreenshots(data, game);

                return game;
            }
        }

        private static void LoadScreenshots(JsonObject data, Game game)
        {
            IJsonValue value;

            if (!data.TryGetValue("screenshots", out value) ||
                value.ValueType != JsonValueType.Array)
                return;

            foreach (IJsonValue screenshotValue in value.GetArray())
            {
                if (screenshotValue.ValueType != JsonValueType.Object)
                    continue;

                JsonObject screenshot = screenshotValue.GetObject();

                string imageUrl = ReadString(screenshot, "path_thumbnail");

                if (string.IsNullOrWhiteSpace(imageUrl))
                    imageUrl = ReadString(screenshot, "path_full");

                if (string.IsNullOrWhiteSpace(imageUrl))
                    continue;

                try
                {
                    game.Screenshots.Add(
                        new BitmapImage(new Uri(imageUrl)));
                }
                catch
                {
                }
            }
        }

        private static string ReadDetailedPrice(JsonObject data)
        {
            IJsonValue value;

            if (!data.TryGetValue("price_overview", out value) ||
                value.ValueType != JsonValueType.Object)
                return "Price unavailable";

            return ReadString(value.GetObject(), "final_formatted");
        }

        private static string ReadReleaseDate(JsonObject data)
        {
            IJsonValue value;

            if (!data.TryGetValue("release_date", out value) ||
                value.ValueType != JsonValueType.Object)
                return "Release date unavailable";

            string date = ReadString(value.GetObject(), "date");

            return string.IsNullOrWhiteSpace(date)
                ? "Release date unavailable"
                : date;
        }

        private static string ReadStringArray(
            JsonObject obj, string key)
        {
            IJsonValue value;

            if (!obj.TryGetValue(key, out value) ||
                value.ValueType != JsonValueType.Array)
                return "Unknown";

            var names = new List<string>();

            foreach (IJsonValue item in value.GetArray())
            {
                if (item.ValueType == JsonValueType.String)
                    names.Add(item.GetString());
            }

            return names.Count == 0
                ? "Unknown"
                : string.Join(", ", names);
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
            IJsonValue value;

            if (!item.TryGetValue("price", out value) ||
                value.ValueType != JsonValueType.Object)
                return "Price unavailable";

            string price = ReadString(
                value.GetObject(), "final_formatted");

            return string.IsNullOrWhiteSpace(price)
                ? "Price unavailable"
                : price;
        }
    }
}