using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    /// <summary>
    /// This class defines the commands that can be passed to commercetools API.
    /// </summary>
    /// <typeparam name="T">The domain specific type.</typeparam>
    public abstract class Command<T> : ICommand<T>
    {
        /// <summary>
        /// Gets or sets the additional parameters.
        /// </summary>
        /// <value>
        /// The additional parameters.
        /// </value>
        public IAdditionalParameters AdditionalParameters { get; set; }

        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        /// <remarks>
        /// This was created to make the code more readable in classes which parse commands with different input and return types.
        /// </remarks>
        public abstract System.Type ResourceType { get; }

        public System.Type ResultType => typeof(T);
    }
}
