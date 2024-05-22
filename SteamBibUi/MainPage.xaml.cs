using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using SteamBibUi.LoginPages;
using SteamBibUi.Models;
using System;
using System.Reflection;

namespace SteamBibUi
{
    public sealed partial class MainPage : Page
    {
        public static User CurrentUser { get; set; }
        private NavigationViewItem lastItem;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CurrentUser = e.Parameter as User;

            switch (CurrentUser.StatusId)
            {
                case 0:
                    UserView.Visibility = Visibility.Visible;
                    NavigateToView("OtherPages.DashboardPage", UserFrame);
                    break;
                case 1:
                    AdminView.Visibility = Visibility.Visible;
                    NavigateToView("OtherPages.DashboardPage", AdminFrame);
                    break;
            }
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var item = args.InvokedItemContainer as NavigationViewItem;
            if (item == null || item == lastItem || item.Tag == null)
                return;

            var clickedView = item.Tag.ToString();
            if (!NavigateToView(clickedView)) return;
            lastItem = item;
        }

        private bool NavigateToView(string clickedView)
        {
            var view = Assembly.GetExecutingAssembly().GetType($"SteamBibUi.{clickedView}");

            if (string.IsNullOrWhiteSpace(clickedView) || view == null)
                return false;

            var contentFrame = GetCorrectNavBar(CurrentUser.StatusId);
            if (contentFrame == null)
                return false;

            contentFrame.Navigate(view, null, new EntranceNavigationTransitionInfo());
            return true;
        }

        private void NavigateToView(string clickedView, Frame frame)
        {
            var view = Assembly.GetExecutingAssembly().GetType($"SteamBibUi.{clickedView}");

            if (!string.IsNullOrWhiteSpace(clickedView) && view != null && frame != null)
            {
                frame.Navigate(view, null, new EntranceNavigationTransitionInfo());
            }
        }

        private Frame GetCorrectNavBar(int statusId)
        {
            switch (statusId)
            {
                case 0:
                    return UserFrame;
                case 1:
                    return AdminFrame;
                default:
                    return null;
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }
    }
}
