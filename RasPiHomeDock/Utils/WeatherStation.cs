using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static RasPiHomeDock.Utils.Constants;

namespace RasPiHomeDock.Utils {
    public class WeatherStation {
        internal WeatherStation(JToken json) {
            Station = json.SelectToken("station").Value<string>();
            Name = json.SelectToken("name").Value<string>();
            Longitude = json.SelectToken("long").Value<float>();
            Latitude = json.SelectToken("lati").Value<float>();
            Altitude = json.SelectToken("alti").Value<float>();
        }

        public string Station { get; }

        public string Name { get; }

        public float Longitude { get; }

        public float Latitude { get; }

        public float Altitude { get; }

        public bool IsWorking { get; private set; } = true;

        public async Task<WeatherMeasurement> GetCurrentMeasurment(bool force = false) {
            if ( !IsWorking && !force ) return null;

            string response = await Http.Client.GetStringAsync(WeatherStationDataUrl + Station);

            JArray meausures = JArray.Parse(response);

            if ( meausures.Count == 0 ) {
                IsWorking = false;
                return null;
            } else if ( !IsWorking ) IsWorking = true;

            return new WeatherMeasurement(meausures[0], stations_);
        }

        public static async Task<WeatherStationList> GetAllStations() {
            if ( stations_.Count == 0 )
                await GetStations();

            return stations_;
        }

        public static async Task<WeatherStation> GetStation(string stationS) {
            if ( stations_.Count == 0 )
                await GetStations();

            return stations_.FirstOrDefault(s => s.Station == stationS) ??
                   stations_.FirstOrDefault(s => s.Name == stationS);
        }

        private static async Task GetStations() {
            string response = await Http.Client.GetStringAsync(WeatherStationInfoUrl);

            foreach ( JToken json in JArray.Parse(response) )
                stations_.Add(new WeatherStation(json));
        }

        private static readonly WeatherStationList stations_ = new WeatherStationList();
    }
}