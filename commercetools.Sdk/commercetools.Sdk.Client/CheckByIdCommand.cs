using commercetools.Sdk.Domain.Common;

namespace commercetools.Sdk.Client
{
    public class CheckByIdCommand<T> : CheckCommand<T>
        where T : Resource<T>, ICheckable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckByIdCommand{T}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public CheckByIdCommand(string id)
        {
            this.Init(id);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckByIdCommand{T}"/> class.
        /// </summary>
        /// <param name="identifiable">The identifying resource</param>
        public CheckByIdCommand(IIdentifiable<T> identifiable)
        {
            this.Init(identifiable.Id);
        }

        private void Init(string id)
        {
            this.ParameterKey = Parameters.Id;
            this.ParameterValue = id;
        }
    }
}
