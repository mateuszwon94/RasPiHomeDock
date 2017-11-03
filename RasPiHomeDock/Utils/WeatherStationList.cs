using System.Collections.ObjectModel;
using System.Linq;
using RasPiHomeDock.Utils.Exceptions;

namespace RasPiHomeDock.Utils {
    public class WeatherStationList : ObservableCollection<WeatherStation> {
        public WeatherStation this[string station]
            => this.FirstOrDefault(s => s.Station == station) ?? throw new NoSuchStationException();
    }
}