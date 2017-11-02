using System;
using System.Collections.Generic;
using System.Linq;
using RasPiHomeDock.Utils.Exceptions;

namespace RasPiHomeDock.Utils {
    public class WeatherStationList : List<WeatherStation> {
        public WeatherStation this[string station]
            => this.FirstOrDefault(s => s.Station == station) ?? throw new NoSuchStationException();
    }
}