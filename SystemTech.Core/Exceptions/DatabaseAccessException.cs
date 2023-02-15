using System;

namespace SystemTech.Core.Exceptions
{
    [Serializable]
    public class DatabaseAccessException : Exception
    {
        public DatabaseAccessException()
        {
        }

        public DatabaseAccessException(string message)
            : base(message)
        {
        }

        public DatabaseAccessException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}