using System;

namespace DarkSecurity
{
    public class HashException : Exception {
        public HashException(string message, Exception inner) : base(message, inner) { }
    }
}
