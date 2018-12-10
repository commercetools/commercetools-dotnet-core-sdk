namespace commercetools.Sdk.Client
{
    using System;
    using System.Collections.Generic;
    using Domain;

    public class GetCommand<T> : Command<T>
    {
        protected GetCommand()
        {
            this.Expand = new List<string>();
        }

        protected GetCommand(List<Expansion<T>> expandPredicates)
            : this()
        {
            if (expandPredicates == null)
            {
                return;
            }

            foreach (var expand in expandPredicates)
            {
                this.Expand.Add(expand.ToString());
            }
        }

        public List<string> Expand { get; }

        public string ParameterKey { get; protected set; }

        public object ParameterValue { get; protected set; }

        public override System.Type ResourceType => typeof(T);
    }
}