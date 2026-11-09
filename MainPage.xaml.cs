using System;
using System.Collections.ObjectModel;
using SteamX.Models;
using SteamX.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SteamX
{
    public sealed partial class MainPage : Page
    {
        private readonly SteamStoreService StoreService =
            new SteamStoreService();

        private readonly ObservableCollection<Game> SearchResults =
            new ObservableCollection<Game>();

        public MainPage()
        {
            InitializeComponent();
            SearchResultsList.ItemsSource = SearchResults;
        }

        private async void SearchButton_Click(
            object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(query))
            {
                StatusText.Text = "Enter a game name.";
                return;
            }

            SearchButton.IsEnabled = false;
            StatusText.Text = "Searching Steam...";
            SearchResults.Clear();

            try
            {
                var games = await StoreService.SearchGamesAsync(query);

                foreach (Game game in games)
                    SearchResults.Add(game);

                StatusText.Text = games.Count + " games found.";
            }
            catch (Exception ex)
            {
                StatusText.Text = "Search failed: " + ex.Message;
            }
            finally
            {
                SearchButton.IsEnabled = true;
            }
        }
    }
}
