using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SteamBibUi.LoginPages;
using SteamBibUi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace SteamBibUi.OtherPages
{
    public sealed partial class AdminConsolePage : Page
    {
        public AdminConsolePage()
        {
            this.InitializeComponent();
        }

        private async void FillDbButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isDatabaseFilled = ApplicationData.Current.LocalSettings.Values.ContainsKey("IsDatabaseFilled");

                if (isDatabaseFilled)
                {
                    ContentDialog alreadyFilledDialog = new ContentDialog
                    {
                        Title = "Database Already Filled",
                        Content = "Click 'Ok' to continue.",
                        CloseButtonText = "Ok",
                        XamlRoot = this.XamlRoot
                    };

                    await alreadyFilledDialog.ShowAsync();
                }
                else
                {
                    var apiHandler = new ApiHandler();
                    await apiHandler.FillSteamAppsAsync();

                    ApplicationData.Current.LocalSettings.Values["IsDatabaseFilled"] = true;

                    ContentDialog successDialog = new ContentDialog
                    {
                        Title = "Database Filled Successfully",
                        Content = "The database has been successfully filled with Steam apps data.",
                        CloseButtonText = "Ok",
                        XamlRoot = this.XamlRoot
                    };

                    await successDialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"An error occurred while filling the database: {ex.Message}",
                    CloseButtonText = "Ok",
                    XamlRoot = this.XamlRoot
                };

                await errorDialog.ShowAsync();
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AdminDashboardPage));
        }
    }
}
