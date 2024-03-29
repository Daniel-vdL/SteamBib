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
        private List<SteamApp> steamApps;
        private List<AppDetails> AppDetailsList;

        public AppViewPage()
        {
            this.InitializeComponent();
            LoadApps();
        }

        public async void LoadApps()
        {
            var apiHandler = new ApiHandler();
            steamApps = await apiHandler.GetSteamAppsAsync();

            var filteredApps = steamApps.Where(steamApp => !string.IsNullOrEmpty(steamApp.Name)).ToList();
            AppsListView.ItemsSource = filteredApps;
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

        public void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchInput = SearchTextBox.Text.ToLower();

            var filteredApps = steamApps.Where(steamApp => steamApp.Name.ToLower().Contains(searchInput) && !string.IsNullOrEmpty(steamApp.Name)).ToList();
            AppsListView.ItemsSource = filteredApps;
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DashboardPage));
        }
    }
}
