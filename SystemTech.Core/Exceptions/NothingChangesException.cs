using System;

namespace SystemTech.Core.Exceptions
{
    [Serializable]
    public class NothingChangesException : Exception
    {
        public NothingChangesException()
        {
        }

        public NothingChangesException(string message) : base(message)
        {
        }

        public NothingChangesException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}