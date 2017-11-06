//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the Microsoft Public License.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.ComponentModel;
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Graph;

namespace RasPiHomeDock.Utils {
    public class WiFiNetwork : INotifyPropertyChanged {
        public WiFiNetwork(WiFiAvailableNetwork network, WiFiAdapter adapter) {
            Network = network;
            adapter_ = adapter;

            UpdateWiFiImage();
            UpdateConnectivityLevel();
        }

        public async void UpdateConnectivityLevel() {
            string connectivityLevel = "Not Connected";
            string connectedSsid = null;

            var connectedProfile = await adapter_.NetworkAdapter.GetConnectedProfileAsync();
            if ( connectedProfile != null &&
                 connectedProfile.IsWlanConnectionProfile &&
                 connectedProfile.WlanConnectionProfileDetails != null ) {
                connectedSsid = connectedProfile.WlanConnectionProfileDetails.GetConnectedSsid();
            }

            if ( !string.IsNullOrEmpty(connectedSsid) ) {
                if ( connectedSsid.Equals(Network.Ssid) ) {
                    connectivityLevel = connectedProfile.GetNetworkConnectivityLevel().ToString();
                }
            }

            ConnectivityLevel = connectivityLevel;

            OnPropertyChanged("ConnectivityLevel");
        }

        public string Ssid => Network.Ssid;

        public string Bssid => Network.Bssid;

        public string ChannelCenterFrequency => $"{Network.ChannelCenterFrequencyInKilohertz}kHz";

        public string Rssi => $"{Network.NetworkRssiInDecibelMilliwatts}dBm";

        public string SecuritySettings => $"Authentication: {Network.SecuritySettings.NetworkAuthenticationType}; Encryption: {Network.SecuritySettings.NetworkEncryptionType}";

        public string ConnectivityLevel { get; private set; }

        public Symbol WiFiSignalBarsIcon { get; private set; }

        public Symbol WiFiProtectionIcon => !IsSecure ? Symbol.ReportHacked : Symbol.View;

        public bool IsSecure => Network.SecuritySettings.NetworkAuthenticationType !=
                                NetworkAuthenticationType.Open80211;

        public WiFiAvailableNetwork Network { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void UpdateWiFiImage() {
            if ( Network.SignalBars > 3 )
                WiFiSignalBarsIcon = Symbol.FourBars;
            else if ( Network.SignalBars > 2 )
                WiFiSignalBarsIcon = Symbol.ThreeBars;
            else if ( Network.SignalBars > 1 )
                WiFiSignalBarsIcon = Symbol.TwoBars;
            else if ( Network.SignalBars > 0 )
                WiFiSignalBarsIcon = Symbol.OneBar;
            else
                WiFiSignalBarsIcon = Symbol.ZeroBars;

            OnPropertyChanged("WiFiImage");
        }

        private readonly WiFiAdapter adapter_;
    }
}