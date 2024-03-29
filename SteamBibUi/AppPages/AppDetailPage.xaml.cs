using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SteamBibUi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace SteamBibUi.AppPages
{
    public sealed partial class AppDetailPage : Page
    {
        private Models.SteamApp selectedApp;
        public AppData appDetails { get; set; }

        public AppDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Models.SteamApp steamApp)
            {
                selectedApp = steamApp;
                LoadAppDetail();
            }
        }

        public async void LoadAppDetail()
        {
            if (selectedApp == null)
                return;

            string url = $"https://store.steampowered.com/api/appdetails?appids={selectedApp.Appid}";

            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var receivedContent = JsonSerializer.Deserialize<Dictionary<string, AppDetails>>(content, options);

            if (receivedContent != null && receivedContent.ContainsKey(selectedApp.Appid.ToString()))
            {
                var appDetail = receivedContent[selectedApp.Appid.ToString()];
                if (appDetail.Data != null && appDetail.Data.Background != null && IsImageUri(appDetail.Data.Background))
                {
                    // Load the details into the UI
                    appDetails = appDetail.Data;
                    Bindings.Update();
                }
                else
                {
                    ContentDialog ErrorDialog = new ContentDialog
                    {
                        Title = "This App does not have the correct data!",
                        Content = "Click 'Ok' to continue",
                        CloseButtonText = "Ok",
                        XamlRoot = this.XamlRoot,
                    };

                    ContentDialogResult result = await ErrorDialog.ShowAsync();

                    this.Frame.Navigate(typeof(AppViewPage));
                }
            }
        }

        private bool IsImageUri(string uriString)
        {
            if (Uri.TryCreate(uriString, UriKind.Absolute, out Uri uri))
            {
                return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
            }
            return false;
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AppViewPage));
        }
    }
}
