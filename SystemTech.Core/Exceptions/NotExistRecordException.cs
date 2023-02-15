using System;

namespace SystemTech.Core.Exceptions
{
    [Serializable]
    public class NotExistRecordException : Exception
    {
        public NotExistRecordException()
        {
        }

        public NotExistRecordException(string message) : base(message)
        {
        }

        public NotExistRecordException(string message, Exception inner) : base(message, inner)
        {
        }
    }

}
