using System;

namespace SystemTech.Core.Exceptions
{
    public class FailedValidationException : Exception
    {
        public FailedValidationException()
        {
        }

        public FailedValidationException(string nameProperty, string message) : base(message)
        {
            NameProperty = nameProperty;
        }
        public string NameProperty { get; }
    }
}