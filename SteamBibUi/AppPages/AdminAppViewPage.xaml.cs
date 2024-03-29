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
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace SteamBibUi.AppPages
{
    public sealed partial class AdminAppViewPage : Page
    {
        public AdminAppViewPage()
        {
            this.InitializeComponent();
            LoadApps();
        }

        public async void LoadApps()
        {
            var apiHandler = new ApiHandler();
            var steamApps = await apiHandler.GetSteamAppsAsync();

            var filteredGames = steamApps.Where(steamApp => !string.IsNullOrEmpty(steamApp.Name)).ToList();
            AppsListView.ItemsSource = filteredGames;
        }

        private async void DeleteApp_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            SteamApp appToDelete = deleteButton?.DataContext as SteamApp;

            if (appToDelete != null)
            {
                ApiHandler apiHandler = new ApiHandler();
                bool deleted = await apiHandler.DeleteSteamAppAsync(appToDelete.Id);

                if (deleted)
                {
                    ContentDialog successDialog = new ContentDialog
                    {
                        Title = "App Deleted Successfully",
                        Content = "The app has been successfully deleted from the database",
                        CloseButtonText = "Ok",
                        XamlRoot = this.XamlRoot
                    };

                    await successDialog.ShowAsync();
                    LoadApps();
                }
                else
                {
                    ContentDialog errorDialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = $"An error occurred while deleting the app",
                        CloseButtonText = "Ok",
                        XamlRoot = this.XamlRoot
                    };

                    await errorDialog.ShowAsync();
                }
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AdminDashboardPage));
        }
    }
}
