using System;
using commercetools.Sdk.Domain.Errors;

namespace commercetools.Sdk.HttpApi.Domain.Exceptions
{
    /// <summary>
    /// This error occurs if you try to update or delete a resource using an outdated version 
    /// </summary>
    /// <seealso cref="commercetools.Sdk.HttpApi.Domain.Exceptions.ClientErrorException" />
    public class ConcurrentModificationException : ClientErrorException
    {
        public override int StatusCode => 409;
        
        public ConcurrentModificationException() 
        {
        }
        /// <summary>
        /// Gets the version of the object at the time of the failed command.
        /// </summary>
        /// <returns>Current Version of the resource</returns>
        public int? GetCurrentVersion()
        {
            int? currentVersion = null;
            if (ErrorResponse != null && ErrorResponse.Errors != null && ErrorResponse.Errors.Count == 1)
            {
                if (ErrorResponse.Errors[0] is ConcurrentModificationError error)
                {
                    currentVersion = error.CurrentVersion;
                }
            }
            return currentVersion;
        }
    }
}