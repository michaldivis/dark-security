using System;

namespace DarkSecurity
{
    public class CryptException : Exception
    {
        public CryptException(string message, Exception inner) : base(message, inner) { }
    }
}
