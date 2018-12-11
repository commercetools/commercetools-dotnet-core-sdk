using commercetools.Sdk.Linq;
using commercetools.Sdk.Util;
using System.Linq.Expressions;

namespace commercetools.Sdk.Domain
{
    public abstract class Facet<T>
    {
        public bool IsCountingProducts { get; set; }
        public string Alias { get; set; }
        public Expression Expression { get; protected set; }

        public override string ToString()
        {
            string facetPath = ServiceLocator.Current.GetService<IFilterExpressionVisitor>().Render(this.Expression);
            if (this.Alias != null)
            {
                facetPath += $" as {this.Alias}";
            }
            if (this.IsCountingProducts)
            {
                facetPath += $" counting products";
            }

            return facetPath;
        }
    }
}