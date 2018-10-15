namespace commercetools.Sdk.Client
{
    using commercetools.Sdk.Domain;
    using System.Collections.Generic;

    public abstract class UpdateCommand<T> : Command<T>
    {
        public int Version { get; protected set; }
        public List<UpdateAction> UpdateActions { get; protected set; }
        public string ParameterKey { get; protected set; }
        public object ParameterValue { get; protected set; }
    }
}
