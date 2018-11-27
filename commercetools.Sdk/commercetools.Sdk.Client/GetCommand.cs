namespace commercetools.Sdk.Client
{
    using commercetools.Sdk.Domain;
    using System.Collections.Generic;

    public abstract class GetCommand<T> : Command<T>
    {
        // An empty constructor is needed for the derived classes
        public GetCommand()
        {
        }

        public GetCommand(List<Expansion<T>> expand)
        {
            this.Expand = expand;
        }

        public List<Expansion<T>> Expand { get; private set; }
        public string ParameterKey { get; protected set; }
        public object ParameterValue { get; protected set; }
    }
}