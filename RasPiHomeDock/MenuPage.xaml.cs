using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using RasPiHomeDock.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RasPiHomeDock {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MenuPage : Page {
        public MenuPage() {
            this.InitializeComponent();

            timer_.Interval = TimeSpan.FromMilliseconds(900);
            timer_.Tick += ReloadDateTime;
        }

        private void ReloadDateTime(object sender, object e) {
            DateTime now = DateTime.Now;
            DateTextBlock.Text = now.ToString("dd-MM-yyyy");
            TimeTextBlock.Text = now.ToString("HH:mm:ss");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            timer_.Start();
        }

        private void HamburgerButton_OnClick(object sender, RoutedEventArgs e) =>
            HamburgerSplitView.IsPaneOpen = !HamburgerSplitView.IsPaneOpen;

        private void HamburgerListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if ( HamburgerSplitView.IsPaneOpen )
                HamburgerSplitView.IsPaneOpen = false;

            if ( WeatherListBoxItem.IsSelected ) {
                MainFrame.Navigate(typeof(WeatherView), HeaderTextBlock);
            } else if ( SettingsListBoxItem.IsSelected ) {
                MainFrame.Navigate(typeof(SettingsView), HeaderTextBlock);
            } else if ( CreditsListBoxItem.IsSelected ) {
                MainFrame.Navigate(typeof(CreditsView), HeaderTextBlock);
            }
        }

        private readonly DispatcherTimer timer_ = new DispatcherTimer();
    }
}