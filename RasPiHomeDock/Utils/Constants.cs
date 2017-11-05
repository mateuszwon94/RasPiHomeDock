using System.Net.Http;

namespace RasPiHomeDock.Utils {
    public static class Constants {
        public const string WeatherStationInfoUrl = "http://mech.fis.agh.edu.pl/meteo/rest/json/info/";
        public const string WeatherStationDataJsonUrl = "http://mech.fis.agh.edu.pl/meteo/rest/json/last/";
        public const string WeatherStationDataXmlUrl = "http://meteo.ftj.agh.edu.pl/meteo/meteo.xml";

        public static class Http {
            public static readonly HttpClient Client = new HttpClient();
        }
    }
}