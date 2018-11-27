using System.Collections.Generic;

namespace commercetools.Sdk.Linq
{
    public abstract class FilterVisitor
    {
        public List<string> Accessors { get; protected set; }

        public abstract string Render();

        public abstract string RenderValue();
    }
}