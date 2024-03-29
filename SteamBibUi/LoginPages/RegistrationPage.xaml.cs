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
using System.Text.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace SteamBibUi.LoginPages
{
    public sealed partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            this.InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextbox.Text;
            var passwordBox = PasswordTextbox;
            var password = passwordBox.Password;

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                using var client = new HttpClient();

                var user = new User
                {
                    Username = username,
                    Password = password
                };

                var userJson = JsonSerializer.Serialize(user);

                var context = new StringContent(userJson, System.Text.Encoding.UTF8, "Application/Json");

                var response = await client.PostAsync("https://localhost:7099/api/Users", context);

                if (response.IsSuccessStatusCode == false)
                {
                    ContentDialog ErrorDialog = new ContentDialog
                    {
                        Title = "Registration failed!",
                        Content = "Click 'Ok' to continue",
                        CloseButtonText = "Ok",
                        XamlRoot = this.XamlRoot,
                    };

                    ContentDialogResult result = await ErrorDialog.ShowAsync();

                    return;
                }


                var answerJson = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var answerUser = JsonSerializer.Deserialize<User>(answerJson, options);

                User.CurrentUser = answerUser;

                this.Frame.Navigate(typeof(DashboardPage));

            }
            else
            {
                ContentDialog ErrorDialog = new ContentDialog
                {
                    Title = "Please check your input",
                    Content = "Click 'Ok' to continue",
                    CloseButtonText = "Ok",
                    XamlRoot = this.XamlRoot,
                };

                ContentDialogResult result = await ErrorDialog.ShowAsync();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }
    }
}
