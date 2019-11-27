namespace CarcassonneDiscovery.Messaging
{
    using System;

    /// <summary>
    /// Exception when trying to map DTO with invalid values to other entities.
    /// </summary>
    public class InvalidDtoValuesException : ArgumentException
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="msg">Error message</param>
        public InvalidDtoValuesException(string msg) : base(msg)
        {
        }
    }
}
