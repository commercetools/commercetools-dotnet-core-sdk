using commercetools.Sdk.Domain;

namespace commercetools.Sdk.Client
{
    public interface ICommand<T>
    {
        /// <summary>
        /// Gets or sets the additional parameters.
        /// </summary>
        /// <value>
        /// The additional parameters.
        /// </value>
        IAdditionalParameters AdditionalParameters { get; set; }

        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        /// <remarks>
        /// This was created to make the code more readable in classes which parse commands with different input and return types.
        /// </remarks>
        System.Type ResourceType { get; }

        System.Type ResultType { get; }
    }
}