using System;
using System.Collections.ObjectModel;
using SteamX.Models;
using SteamX.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;

namespace SteamX
{
    public sealed partial class MainPage : Page
    {
        private readonly SteamStoreService StoreService =
            new SteamStoreService();

        private readonly ObservableCollection<Game> SearchResults =
            new ObservableCollection<Game>();

        private bool IsLoadingDetails = false;

        public MainPage()
        {
            InitializeComponent();

            SearchResultsList.ItemsSource = SearchResults;

            SystemNavigationManager.GetForCurrentView()
                .BackRequested += OnBackRequested;
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

        private async void SearchResultsList_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (IsLoadingDetails)
                return;

            Game selectedGame =
                SearchResultsList.SelectedItem as Game;

            if (selectedGame == null)
                return;

            IsLoadingDetails = true;

            StoreView.Visibility = Visibility.Collapsed;
            DetailsView.Visibility = Visibility.Visible;

            DetailsName.Text = selectedGame.Name;
            DetailsPrice.Text = selectedGame.Price;
            DetailsArtwork.Source = selectedGame.Artwork;

            DetailsDescription.Text = "Loading description...";
            DetailsDeveloper.Text = "Developer: Loading...";
            DetailsPublisher.Text = "Publisher: Loading...";
            DetailsReleaseDate.Text = "Release date: Loading...";
            DetailsStatus.Text = "Loading game information from Steam...";

            ScreenshotsList.ItemsSource = null;

            try
            {
                Game details =
                    await StoreService.GetGameDetailsAsync(
                        selectedGame.AppId);

                if (details == null)
                {
                    DetailsStatus.Text =
                        "Steam did not provide details for this game.";

                    return;
                }

                DetailsName.Text = details.Name;
                DetailsPrice.Text = details.Price;

                if (details.HeaderArtwork != null)
                {
                    DetailsArtwork.Source =
                        details.HeaderArtwork;
                }

                DetailsDescription.Text =
                    string.IsNullOrWhiteSpace(details.Description)
                    ? "Description unavailable."
                    : details.Description;

                DetailsDeveloper.Text =
                    "Developer: " + details.Developers;

                DetailsPublisher.Text =
                    "Publisher: " + details.Publishers;

                DetailsReleaseDate.Text =
                    "Release date: " + details.ReleaseDate;

                ScreenshotsList.ItemsSource =
                    details.Screenshots;

                DetailsStatus.Text = "Game information loaded.";

            }
            catch (Exception ex)
            {
                DetailsStatus.Text =
                    "Unable to load details: " + ex.Message;
            }
            finally
            {
                IsLoadingDetails = false;
            }
        }

        private void BackButton_Click(
            object sender, RoutedEventArgs e)
        {
            GoBackToStore();
        }

        private void OnBackRequested(
            object sender, BackRequestedEventArgs e)
        {
            if (DetailsView.Visibility == Visibility.Visible)
            {
                e.Handled = true;
                GoBackToStore();
            }
        }

        private void GoBackToStore()
        {
            DetailsView.Visibility = Visibility.Collapsed;
            StoreView.Visibility = Visibility.Visible;

            SearchResultsList.SelectedItem = null;
            SearchResultsList.Focus(
                Windows.UI.Xaml.FocusState.Programmatic);
        }
    }
}