using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using RasPiHomeDock.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RasPiHomeDock.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeatherView : Page {
        public WeatherView() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            if ( e.Parameter is TextBlock headerTextBlock ) headerTextBlock.Text = "Pogoda";

            StationListView.ItemsSource = weatherStations_;
            if (weatherStations_.Count == 0) RefreshWeatherStationListBox();
        }

        private async void RefreshWeatherStationListBox() {
            weatherStations_.Clear();

            foreach ( WeatherStation station in await WeatherStation.GetAllStations() )
                weatherStations_.Add(station);
        }

        private readonly WeatherStationList weatherStations_ = new WeatherStationList();

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e) {
            RefreshWeatherStationListBox();
        }
    }
}