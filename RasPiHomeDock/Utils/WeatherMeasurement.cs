using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using RasPiHomeDock.Utils.Exceptions;

namespace RasPiHomeDock.Utils {
    public class WeatherMeasurement {
        internal WeatherMeasurement(JToken json, IEnumerable<WeatherStation> stations) {
            Time = json.SelectToken("time").Value<DateTime>();
            UtcTime = json.SelectToken("utc").Value<DateTime>();
            string station = json.SelectToken("station").Value<string>();
            Station = stations.FirstOrDefault(s => s.Station == station) ?? throw new NoSuchStationException();

            Measures = new Data(json.SelectToken("data"));
        }

        public DateTime Time { get; }

        public WeatherStation Station { get; }

        public DateTime UtcTime { get; }

        public Data Measures { get; }

        public class Data {
            internal Data(JToken json) {
                PreasureAtSeaLevel = json.SelectToken("p0").Value<float?>();
                Temperature = json.SelectToken("ta").Value<float?>();
                DewPoint = json.SelectToken("t0").Value<float?>();
                Humidity = json.SelectToken("ha").Value<float?>();
                RainInLastHour = json.SelectToken("r1").Value<float?>();
                Rain = json.SelectToken("ra").Value<float?>();
                WindDirection = json.SelectToken("wd").Value<float?>();
                WindSpeed = json.SelectToken("ws").Value<float?>();
                TemporaryWindSpeed = json.SelectToken("wg").Value<float?>();
            }

            public float? PreasureAtSeaLevel { get; }

            public float? Temperature { get; }

            public float? DewPoint { get; }

            public float? Humidity { get; }

            public float? RainInLastHour { get; }

            public float? Rain { get; }

            public float? WindDirection { get; }

            public float? WindSpeed { get; }

            public float? TemporaryWindSpeed { get; }
        }
    }
}