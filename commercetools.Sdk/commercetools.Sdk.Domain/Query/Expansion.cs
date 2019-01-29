using System.Linq.Expressions;
using commercetools.Sdk.Linq;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Domain.Query
{
    public class Expansion<T>
    {
        public Expansion(Expression expression)
        {
            this.Expression = expression;
        }

        public Expression Expression { get; set; }

        public override string ToString()
        {
            return ServiceLocator.Current.GetService<IExpansionExpressionVisitor>().GetPath(this.Expression);
        }
    }
}