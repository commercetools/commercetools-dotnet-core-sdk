namespace commercetools.Sdk.Client
{
    using commercetools.Sdk.Domain;
    using System.Collections.Generic;

    public abstract class GetCommand<T> : Command<T>
    {

        public GetCommand()
        {
        }

        public GetCommand(List<Expansion<T>> expandPredicates)
        {
            if (expandPredicates != null)
            {
                this.Expand = new List<string>();
                foreach (var expand in expandPredicates)
                {
                    this.Expand.Add(expand.ToString());
                }
            }
        }

        public List<string> Expand { get; private set; }
        public string ParameterKey { get; protected set; }
        public object ParameterValue { get; protected set; }
    }
}