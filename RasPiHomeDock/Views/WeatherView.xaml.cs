using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.QueryStringDotNET; 
using RasPiHomeDock.Utils;
using RasPiHomeDock.Utils.Exceptions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RasPiHomeDock.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeatherView : Page {
        public WeatherView() {
            this.InitializeComponent();

            timer_.Interval = TimeSpan.FromSeconds(30);
            timer_.Tick += RefreshDataFromStation;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            if ( e.Parameter is TextBlock headerTextBlock ) headerTextBlock.Text = "Pogoda";

            StationListView.ItemsSource = WeatherStation.Stations;
            if ( WeatherStation.Stations.Count == 0) await RefreshWeatherStationListBox();
            StationListView.SelectedIndex = 0;
        }

        private async Task RefreshWeatherStationListBox() {
            WeatherStation.Stations.Clear();

            await WeatherStation.RefreshStations();
        }

        private void StationListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            timer_.Stop();

            if ( e.AddedItems[0] is WeatherStation selectetStation ) {
                LongitudeTextBlock.Text = selectetStation.Longitude.ToString(CultureInfo.CurrentCulture);
                LatitudeTextBlock.Text = selectetStation.Latitude.ToString(CultureInfo.CurrentCulture);
                AltitudeTextBlock.Text = selectetStation.Altitude.ToString(CultureInfo.CurrentCulture);
                MeasureTimeTextBlock.Text = string.Empty;
                TemperatureTextBlock.Text = string.Empty;
                WindchillTemperatureTextBlock.Text = string.Empty;
                HumidityTextBlock.Text = string.Empty;
                FallTextBlock.Text = string.Empty;
                HailTextBlock.Text = string.Empty;
                PreasureAtSeaLevelTextBlock.Text = string.Empty;
                WindTextBlock.Text = string.Empty;

                RefreshDataFromStation(null, null);
            }
        }

        private async void RefreshDataFromStation(object sender, object e) {
            if ( StationListView.SelectedItem is WeatherStation selectetStation ) {
                try {
                    DataRetrivingProgressRing.IsActive = true;
                    await selectetStation.RefreshMesurment();

                    MeasureTimeTextBlock.Text = selectetStation.Mesurment.Time.ToString("dd-MM-yy HH:mm");
                    TemperatureTextBlock.Text = $"{selectetStation.Mesurment.Temperature} {selectetStation.Mesurment.TemperatureUnits}";
                    WindchillTemperatureTextBlock.Text = $"{selectetStation.Mesurment.WindchillTemperature} {selectetStation.Mesurment.WindchillTemperatureUnits}";
                    HumidityTextBlock.Text = $"{selectetStation.Mesurment.Humidity} {selectetStation.Mesurment.HumidityUnits}";
                    FallTextBlock.Text = $"{selectetStation.Mesurment.Fall} {selectetStation.Mesurment.FallUnits}";
                    HailTextBlock.Text = $"{selectetStation.Mesurment.Hail} {selectetStation.Mesurment.HailUnits}";
                    PreasureAtSeaLevelTextBlock.Text = $"{selectetStation.Mesurment.PreasureAtSeaLevel} {selectetStation.Mesurment.PreasureAtSeaLevelUnits}";
                    WindTextBlock.Text = $"{selectetStation.Mesurment.AverageWindSpeed} {selectetStation.Mesurment.AverageWindSpeedUnits} ({selectetStation.Mesurment.MaxWindSpeed} {selectetStation.Mesurment.MaxWindSpeedUnits})";

                    if ( !timer_.IsEnabled ) timer_.Start();
                } catch ( EmptyResponsdException ) {
                    NoDataFromStationNotification.Show(5000);
                    timer_.Stop();
                } finally {
                    DataRetrivingProgressRing.IsActive = false;
                }
            }
        }

        private readonly DispatcherTimer timer_ = new DispatcherTimer();
    }
}