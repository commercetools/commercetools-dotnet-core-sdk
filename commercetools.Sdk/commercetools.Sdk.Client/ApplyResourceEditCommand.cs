using System;
using commercetools.Sdk.Domain.Common;
using commercetools.Sdk.Domain.OrderEdits;

namespace commercetools.Sdk.Client
{
    public abstract class ApplyResourceEditCommand<T> : Command<T>
    {
        public ApplyResourceEditCommand(string id, int editVersion, int resourceVersion)
        {
            this.Init(id, editVersion, resourceVersion);
        }

        public ApplyResourceEditCommand(IVersioned<T> edit, int resourceVersion)
        {
            this.Init(edit.Id, edit.Version, resourceVersion);
        }

        public override Type ResourceType => typeof(T);

        public string Id { get; set; }

        public int EditVersion { get; set; }

        public int ResourceVersion { get; set; }

        private void Init(string orderEditId, int editVersion, int resourceVersion)
        {
            this.Id = orderEditId;
            this.EditVersion = editVersion;
            this.ResourceVersion = resourceVersion;
        }
    }
}