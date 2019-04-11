using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    // p.Attributes.Any(...)
    public class ListPredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        public int Priority { get; } = 4;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (IsMethodNameAllowed(methodCallExpression) && IsValidMethodCaller(methodCallExpression))
                {
                    return true;
                }
            }

            return false;
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            MethodCallExpression methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression == null)
            {
                return null;
            }

            IPredicateVisitor inner = predicateVisitorFactory.Create(methodCallExpression.Arguments[1]);

            var memberExpression = methodCallExpression.Arguments[0] as MemberExpression;

            // Id
            string currentName = memberExpression.Member.Name;
            ConstantPredicateVisitor constant = new ConstantPredicateVisitor(currentName);
            IPredicateVisitor parent = predicateVisitorFactory.Create(memberExpression);

            return CanBeCombined(parent) ? CombinePredicates(parent, inner) : new ContainerPredicateVisitor(inner, constant);
        }

        // When there is more than one property accessor, the last accessor needs to be taken out and added to the binary logical predicate.
        // property(property(property operator value)
        private static IPredicateVisitor CombinePredicates(IPredicateVisitor left, IPredicateVisitor right)
        {
            var containerLeft = (ContainerPredicateVisitor)left;

            IPredicateVisitor parent = containerLeft;
            if (parent == null)
            {
                return new ContainerPredicateVisitor(right, left);
            }

            IPredicateVisitor innerContainer = right;
            ContainerPredicateVisitor combinedContainer = null;
            while (parent != null)
            {
                if (CanBeCombined(parent))
                {
                    var container = (ContainerPredicateVisitor)parent;
                    innerContainer = new ContainerPredicateVisitor(innerContainer, container.Inner);
                    combinedContainer = new ContainerPredicateVisitor(innerContainer, container.Parent);
                    parent = container.Parent;
                }
            }

            return combinedContainer;
        }

        private static bool CanBeCombined(IPredicateVisitor left)
        {
            return left is ContainerPredicateVisitor;
        }

        private static bool IsMethodNameAllowed(MethodCallExpression expression)
        {
            return expression.Method.Name == "Any";
        }

        private static bool IsValidMethodCaller(MethodCallExpression expression)
        {
            Expression callerExpression = expression.Arguments[0];
            if (callerExpression is MemberExpression memberExpression)
            {
                if (memberExpression.Type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
