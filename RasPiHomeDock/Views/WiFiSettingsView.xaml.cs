using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Windows.Devices.WiFi;
using Windows.System.Profile;
using RasPiHomeDock.Utils;
using static RasPiHomeDock.Utils.DeviceTypeHelper;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace RasPiHomeDock.Views {
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class WiFiSettingsView : Page {
        public WiFiSettingsView() { this.InitializeComponent(); }

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            if ( e.Parameter is TextBlock headerTextBlock )
                headerTextBlock.Text = "Ustawienia WiFi";

            if ( GetDeviceType() == DeviceType.IoT ) {
                var access = await WiFiAdapter.RequestAccessAsync();
                if ( access != WiFiAccessStatus.Allowed ) {
                    WiFiProblemNotification.Show("Dostęp wzbroniony!", 5000);
                } else {
                    var result = await WiFiAdapter.FindAllAdaptersAsync();
                    if ( result.Count >= 1 ) {
                        wiFiAdapter_ = result[0];

                        RefreshWiFi();
                    } else {
                        WiFiProblemNotification.Show("Nie wykryto żadnego adaptera WiFi!", 5000);
                    }
                }
            } else {
                WiFiProblemNotification.Show("Błąd! Twoje urządzenie nie daje możliwości manipulacji WiFi", 5000);
                await Task.Delay(7000);
                WiFiProblemNotification.Show(AnalyticsInfo.VersionInfo.DeviceFamily);
            }
        }

        private async void RefreshWiFi() {
            try {
                await wiFiAdapter_.ScanAsync();
            } catch ( Exception err ) {
                WiFiProblemNotification.Show($"Błąd skanowania sieci WiFi!\nError: {err.HResult} - {err.Message}", 5000);
                return;
            }

            DisplayNetworkReportAsync(wiFiAdapter_.NetworkReport);
        }

        private void DisplayNetworkReportAsync(WiFiNetworkReport report) {
            wifiNetworks_.Clear();

            foreach ( var network in report.AvailableNetworks )
                wifiNetworks_.Add(new WiFiNetwork(network, wiFiAdapter_));
        }

        private WiFiAdapter wiFiAdapter_;
        private readonly ObservableCollection<WiFiNetwork> wifiNetworks_ = new ObservableCollection<WiFiNetwork>();

        private void AvaliableWiFiListView_RefreshCommand(object sender, EventArgs e) { RefreshWiFi(); }

        private void AvaliableWiFiListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            
        }
    }
}