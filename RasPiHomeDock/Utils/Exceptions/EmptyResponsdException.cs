using System;

namespace RasPiHomeDock.Utils.Exceptions {
    public class EmptyResponsdException : Exception {
        public EmptyResponsdException() { }

        public EmptyResponsdException(string message) : base(message) { }

        public EmptyResponsdException(string message, Exception innerException) : base(message, innerException) { }
    }
}