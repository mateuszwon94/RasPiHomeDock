using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using RasPiHomeDock.Utils.Exceptions;

namespace RasPiHomeDock.Utils {
    public class WeatherMesurment {
        internal WeatherMesurment(XmlElement xmlElement) {
            Time = DateTime.Parse(xmlElement.Attributes["data"].InnerText);

            (Temperature, TemperatureUnits) =
                SplitDataAndUnits(xmlElement["ta"].InnerText);
            (WindchillTemperature, WindchillTemperatureUnits) =
                SplitDataAndUnits(xmlElement["owindchill"].InnerText);
            (DewPoint, DewPointUnits) =
                SplitDataAndUnits(xmlElement["odew"].InnerText);
            (PreasureAtSeaLevel, PreasureAtSeaLevelUnits) =
                SplitDataAndUnits(xmlElement["barosealevel"].InnerText);
            (Preasure, PreasureUnits) =
                SplitDataAndUnits(xmlElement["pa"].InnerText);
            (AverageWindSpeed, AverageWindSpeedUnits) =
                SplitDataAndUnits(xmlElement["sm"].InnerText);
            (MaxWindSpeed, MaxWindSpeedUnits) =
                SplitDataAndUnits(xmlElement["sx"].InnerText);
            (WindDirection, WindDirectionUnits) =
                SplitDataAndUnits(xmlElement["dm"].InnerText);
            (Fall, FallUnits) =
                SplitDataAndUnits(xmlElement["rc"].InnerText);
            (FallInLastHour, FallInLastHourUnits) =
                SplitDataAndUnits(xmlElement["ri"].InnerText);
            (Hail, HailUnits) =
                SplitDataAndUnits(xmlElement["hc"].InnerText);
            (HailInLastHour, HailInLastHourUnits) =
                SplitDataAndUnits(xmlElement["hi"].InnerText);
            (Humidity, HumidityUnits) =
                SplitDataAndUnits(xmlElement["ua"].InnerText);
        }

        public DateTime Time { get; }

        public float PreasureAtSeaLevel { get; }

        public float Preasure { get; }

        public float WindchillTemperature { get; }

        public float Temperature { get; }

        public float DewPoint { get; }

        public float Humidity { get; }

        public float FallInLastHour { get; }

        public float Fall { get; }

        public float HailInLastHour { get; }

        public float Hail { get; }

        public float WindDirection { get; }

        public float AverageWindSpeed { get; }

        public float MaxWindSpeed { get; }

        public string PreasureAtSeaLevelUnits { get; }

        public string PreasureUnits { get; }

        public string WindchillTemperatureUnits { get; }

        public string TemperatureUnits { get; }

        public string DewPointUnits { get; }

        public string HumidityUnits { get; }

        public string FallInLastHourUnits { get; }

        public string FallUnits { get; }

        public string HailInLastHourUnits { get; }

        public string HailUnits { get; }

        public string WindDirectionUnits { get; }

        public string AverageWindSpeedUnits { get; }

        public string MaxWindSpeedUnits { get; }

        private static (float data, string units) SplitDataAndUnits(string data) {
            string[] splited = data.Split(' ');
            return (float.Parse(splited[0]), splited[1]);
        }
    }
}