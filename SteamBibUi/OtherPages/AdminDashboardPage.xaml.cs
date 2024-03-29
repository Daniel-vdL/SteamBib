using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SteamBibUi.AppPages;
using SteamBibUi.LoginPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace SteamBibUi.OtherPages
{
    public sealed partial class AdminDashboardPage : Page
    {
        public AdminDashboardPage()
        {
            this.InitializeComponent();
        }

        private void AppPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AppViewPage));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }

        private void AdminConsoleButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AdminConsolePage));
        }
    }
}
