namespace CarcassonneDiscovery.Messaging
{
    using System;

    public class ValidationHelper
    {
        /// <summary>
        /// Checks whether nullable parameter has value.
        /// Throw <see cref="ArgumentException" /> when has no value.
        /// </summary>
        /// <typeparam name="T">Type of nullable struct.</typeparam>
        /// <param name="value">Value to be null-checked.</param>
        /// <param name="errorMsg">Error message.</param>
        public static void CheckHasValue<T>(T? value, string errorMsg = "Expected not null value.")
            where T : struct
        {
            if (!value.HasValue)
            {
                throw new ArgumentException(errorMsg);
            }
        }

        /// <summary>
        /// Checks whether nullable parameter has value.
        /// Throw <see cref="ArgumentException" /> when has no value.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="value">Value to be null-checked.</param>
        /// <param name="errorMsg">Error message.</param>
        public static void CheckHasValue<T>(T value, string errorMsg = "Expected not null value.")
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentException(errorMsg);
            }
        }

        /// <summary>
        /// Checks whether a value is equal to the expected value.
        /// Throw <see cref="ArgumentException" /> when has no value.
        /// </summary>
        /// <param name="expected">Expected value.</param>
        /// <param name="actual">Actual value.</param>
        public static void CheckType(ClientRequestType expected, ClientRequestType actual)
        {
            if (expected != actual)
            {
                throw new ArgumentException($"Expected type {expected}, actually got {actual}.");
            }
        }

        /// <summary>
        /// Checks whether a value is equal to the expected value.
        /// Throw <see cref="ArgumentException" /> when has no value.
        /// </summary>
        /// <param name="expected">Expected value.</param>
        /// <param name="actual">Actual value.</param>
        public static void CheckType(ServerResponseType expected, ServerResponseType actual)
        {
            if (expected != actual)
            {
                throw new ArgumentException($"Expected type {expected}, actually got {actual}.");
            }
        }
    }
}
