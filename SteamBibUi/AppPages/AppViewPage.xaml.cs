using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SteamBibUi.Models;
using SteamBibUi.OtherPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace SteamBibUi.AppPages
{
    public sealed partial class AppViewPage : Page
    {
        private Dictionary<string, string> genres;
        private ObservableCollection<SteamApp> steamApps;
        private List<AppDetails> AppDetailsList;
        public AppData appDetails { get; set; }

        public AppViewPage()
        {
            this.InitializeComponent();
            LoadApps();
            LoadGenres();
        }

        private void LoadGenres()
        {
            string jsonFilePath = @"C:\VisualStudio\SteamBib\SteamBibApi\OtherFiles\genres.json";
            string json = File.ReadAllText(jsonFilePath);
            genres = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            // Populate ComboBox with genres
            foreach (var genre in genres.Values)
            {
                GenreComboBox.Items.Add(genre);
            }
        }

        public async void LoadApps()
        {
            var apiHandler = new ApiHandler();
            var getAppDetails = new GetAppDetails();
            steamApps = new ObservableCollection<SteamApp>(await apiHandler.GetSteamAppsAsync());

            var validApps = new List<SteamApp>();

            foreach (var steamApp in steamApps)
            {
                await getAppDetails.PopulateAppDataAsync(steamApp);

                // Check if the AppData is valid according to your criteria
                if (IsValidAppData(steamApp.AppData))
                {
                    validApps.Add(steamApp);

                    // Break the loop if we have found 100 valid apps
                    if (validApps.Count == 100)
                    {
                        break;
                    }
                }
            }

            AppsListView.ItemsSource = new ObservableCollection<SteamApp>(validApps);
        }

        private void AppsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedApp = (Models.SteamApp)e.ClickedItem;
            Frame.Navigate(typeof(AppDetailPage), selectedApp);
        }

        private void RefreshGames_Click(object sender, RoutedEventArgs e)
        {
            LoadApps();
        }

        public async void UpdateSearch()
        {
            var searchInput = SearchTextBox.Text.ToLower();

            var getAppDetails = new GetAppDetails();

            var filteredApps = steamApps
                .Where(steamApp => steamApp.Name.ToLower().Contains(searchInput) && !string.IsNullOrEmpty(steamApp.Name))
                .Take(50)
                .ToList();

            var validFilteredApps = new List<SteamApp>();

            foreach (var steamApp in filteredApps)
            {
                await getAppDetails.PopulateAppDataAsync(steamApp);

                // Check if the AppData is valid according to your criteria
                if (IsValidAppData(steamApp.AppData))
                {
                    validFilteredApps.Add(steamApp);
                }
            }

            AppsListView.ItemsSource = new ObservableCollection<SteamApp>(validFilteredApps);
        }

        public async void FilterGenre()
        {
            var selectedGenre = (string)GenreComboBox.SelectedItem;
            var getAppDetails = new GetAppDetails();

            var filteredApps = steamApps
                .Where(steamApp => selectedGenre != null && steamApp.AppData != null && steamApp.AppData.Genres != null && steamApp.AppData.Genres.Any(g => g.Description == selectedGenre))
                .Take(50)
                .ToList();

            var validFilteredApps = new List<SteamApp>();

            foreach (var steamApp in filteredApps)
            {
                await getAppDetails.PopulateAppDataAsync(steamApp);

                if (IsValidAppData(steamApp.AppData))
                {
                    validFilteredApps.Add(steamApp);
                }
            }

            AppsListView.ItemsSource = new ObservableCollection<SteamApp>(validFilteredApps);
        }


        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DashboardPage));
        }

        private void SearchTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                UpdateSearch();
            }
        }

        private void GenreComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterGenre();
        }

        private bool IsValidAppData(AppData appData)
        {
            return appData != null && !string.IsNullOrEmpty(appData.Name) && appData.Type == "game";
        }
    }
}
