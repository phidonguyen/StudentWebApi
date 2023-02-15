using System;

namespace SystemTech.Core.Exceptions
{
    [Serializable]
    public class ErrorsTrackingException : Exception
    {
        public ErrorsTrackingException()
        {
        }

        public ErrorsTrackingException(string message) : base(message)
        {
        }

        public ErrorsTrackingException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}