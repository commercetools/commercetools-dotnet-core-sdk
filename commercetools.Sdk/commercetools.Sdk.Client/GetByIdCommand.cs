using System;
using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    /// <summary>
    /// This class defines the command that gets resources by their identifier.
    /// </summary>
    /// <typeparam name="T">The domain specific type.</typeparam>
    /// <seealso cref="commercetools.Sdk.Client.GetCommand{T}" />
    public class GetByIdCommand<T> : GetCommand<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdCommand{T}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public GetByIdCommand(string id)
        {
            this.Init(id);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdCommand{T}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="expand">The list of expansions.</param>
        public GetByIdCommand(string id, List<Expansion<T>> expand)
            : base(expand)
        {
            this.Init(id);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdCommand{T}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="additionalParameters">The additional parameters.</param>
        public GetByIdCommand(string id, IAdditionalParameters<T> additionalParameters)
            : base(additionalParameters)
        {
            this.Init(id);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdCommand{T}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="expand">The list of expansions.</param>
        /// <param name="additionalParameters">The additional parameters.</param>
        public GetByIdCommand(string id, List<Expansion<T>> expand, IAdditionalParameters<T> additionalParameters)
            : base(expand, additionalParameters)
        {
            this.Init(id);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdCommand{T}"/> class.
        /// </summary>
        /// <param name="identifiable">The identifying resource</param>
        public GetByIdCommand(IIdentifiable<T> identifiable)
        {
            this.Init(identifiable.Id);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdCommand{T}"/> class.
        /// </summary>
        /// <param name="identifiable">The identifying resource</param>
        /// <param name="expand">The list of expansions.</param>
        public GetByIdCommand(IIdentifiable<T> identifiable, List<Expansion<T>> expand)
            : base(expand)
        {
            this.Init(identifiable.Id);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdCommand{T}"/> class.
        /// </summary>
        /// <param name="identifiable">The identifying resource</param>
        /// <param name="additionalParameters">The additional parameters.</param>
        public GetByIdCommand(IIdentifiable<T> identifiable, IAdditionalParameters<T> additionalParameters)
            : base(additionalParameters)
        {
            this.Init(identifiable.Id);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdCommand{T}"/> class.
        /// </summary>
        /// <param name="identifiable">The identifying resource</param>
        /// <param name="expand">The list of expansions.</param>
        /// <param name="additionalParameters">The additional parameters.</param>
        public GetByIdCommand(IIdentifiable<T> identifiable, List<Expansion<T>> expand, IAdditionalParameters<T> additionalParameters)
            : base(expand, additionalParameters)
        {
            this.Init(identifiable.Id);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdCommand{T}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public GetByIdCommand(Guid id)
        {
            this.Init(id.ToString());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdCommand{T}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="expand">The list of expansions.</param>
        public GetByIdCommand(Guid id, List<Expansion<T>> expand)
            : base(expand)
        {
            this.Init(id.ToString());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdCommand{T}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="additionalParameters">The additional parameters.</param>
        public GetByIdCommand(Guid id, IAdditionalParameters<T> additionalParameters)
            : base(additionalParameters)
        {
            this.Init(id.ToString());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdCommand{T}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="expand">The list of expansions.</param>
        /// <param name="additionalParameters">The additional parameters.</param>
        public GetByIdCommand(Guid id, List<Expansion<T>> expand, IAdditionalParameters<T> additionalParameters)
            : base(expand, additionalParameters)
        {
            this.Init(id.ToString());
        }

        private void Init(string id)
        {
            this.ParameterKey = Parameters.Id;
            this.ParameterValue = id;
        }
    }
}
