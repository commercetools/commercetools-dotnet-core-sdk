namespace commercetools.Sdk.Client
{
    using System.Collections.Generic;
    using Domain;

    public abstract class UpdateCommand<T> : Command<T>
    {
        protected UpdateCommand()
        {
            this.UpdateActions = new List<UpdateAction<T>>();
        }

        public int Version { get; protected set; }

        public List<UpdateAction<T>> UpdateActions { get; }

        public string ParameterKey { get; protected set; }

        public object ParameterValue { get; protected set; }
    }
}