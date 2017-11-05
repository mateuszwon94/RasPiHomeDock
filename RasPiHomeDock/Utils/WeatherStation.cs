using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using RasPiHomeDock.Utils.Exceptions;
using static RasPiHomeDock.Utils.Constants;

namespace RasPiHomeDock.Utils {
    public class WeatherStation {
        internal WeatherStation(XmlElement xmlElement) {
            Name = xmlElement["nazwa_stacji"].InnerText;
            Longitude = ConvertGeographicalCoordinates(xmlElement["szerokosc_geograficzna"].InnerText);
            Latitude = ConvertGeographicalCoordinates(xmlElement["dlugosc_geograficzna"].InnerText);
            string altString = xmlElement["wysokosc_npm"].InnerText;
            Altitude = float.Parse(altString.Remove(altString.Length - 1));
        }

        private static float ConvertGeographicalCoordinates(string coordinates) {
            string s1 = coordinates.Substring(0, 2);
            string s2 = coordinates.Substring(3, 2);
            int degree = int.Parse(s1),
                minutes = int.Parse(s2);

            float coord = degree + minutes / 60.0f;
            return (coordinates.EndsWith("N") || coordinates.EndsWith("E") ? coord : -coord);
        }

        public string Name { get; }

        public float Longitude { get; }

        public float Latitude { get; }

        public float Altitude { get; }

        public WeatherMesurment Mesurment { get; private set; }

        public static async Task RefreshStations() {
            XmlDocument stationXml = new XmlDocument();
            stationXml.LoadXml(await Http.Client.GetStringAsync(WeatherStationDataXmlUrl));

            Stations.Add(new WeatherStation(stationXml.DocumentElement));
        }

        public async Task RefreshMesurment() {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(await Http.Client.GetStringAsync(WeatherStationDataXmlUrl));

            Mesurment = new WeatherMesurment(xmlDoc.DocumentElement["dane_aktualne"]);
        }

        public static ObservableCollection<WeatherStation> Stations { get; } = new ObservableCollection<WeatherStation>();
    }
}