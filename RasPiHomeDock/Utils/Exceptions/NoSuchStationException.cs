using System;

namespace RasPiHomeDock.Utils.Exceptions {
    public class NoSuchStationException : Exception {
        public NoSuchStationException() { }

        public NoSuchStationException(string message) : base(message) { }

        public NoSuchStationException(string message, Exception innerException) : base(message, innerException) { }
    }
}