using System.Linq.Expressions;
using commercetools.Sdk.Registration;
using commercetools.Sdk.Linq.Filter;

namespace commercetools.Sdk.Domain.ProductProjections
{
    public abstract class Facet<T>
    {
        public bool IsCountingProducts { get; set; }
        public string Alias { get; set; }
        public Expression Expression { get; protected set; }

        public override string ToString()
        {
            string facetPath = ServiceLocator.Current.GetService<IFilterPredicateExpressionVisitor>().Render(this.Expression);
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