using System.Linq.Expressions;

namespace commercetools.Sdk.Domain
{
    public abstract class Facet<T>
    {
        public bool IsCountingProducts { get; set; }
        public string Alias { get; set; }
        public Expression Expression { get; protected set; }
    }
}