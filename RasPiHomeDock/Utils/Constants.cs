using System.Net.Http;

namespace RasPiHomeDock.Utils {
    public static class Constants {
        public const string WeatherStationInfoUrl = "http://mech.fis.agh.edu.pl/meteo/rest/json/info/";
        public const string WeatherStationDataUrl = "http://mech.fis.agh.edu.pl/meteo/rest/json/last/";

        public static class Http {
            public static readonly HttpClient Client = new HttpClient();
        }
    }
}