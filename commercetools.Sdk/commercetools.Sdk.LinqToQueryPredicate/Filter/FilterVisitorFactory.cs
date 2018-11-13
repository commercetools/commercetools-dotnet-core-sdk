using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    public class FilterVisitorFactory
    {
        private List<string> allowedExtensionMethods = new List<string>() { { "Missing" }, { "Exists" } };

        public FilterVisitor Create(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Lambda)
            {
                return Create(((LambdaExpression)expression).Body);
            }
            if (expression is MethodCallExpression methodCallExpression)
            {
                return CreateFilterVisitor(methodCallExpression);
            }
            if (expression.NodeType == ExpressionType.Equal)
            {
                return CreateFilterVisitor((BinaryExpression)expression);
            }
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                return CreateFilterVisitor((MemberExpression)expression);
            }
            if (expression is BinaryExpression)
            {
                return CreateGroupFilterVisitor((BinaryExpression)expression);
            }

            throw new NotSupportedException("The expression type is not supported.");
        }

        private FilterVisitor CreateFilterVisitor(MethodCallExpression expression)
        {
            if (expression.Method.Name == "Any" || expression.Method.Name == "Where" || expression.Method.Name == "Select")
            {
                Expression accessorExpression = expression.Arguments[0];
                if (accessorExpression is MemberExpression memberExpression && memberExpression.Member.Name == "Attributes")
                {
                    AttributeFilterVisitor attributeFilterVisitor = new AttributeFilterVisitor(expression);
                    return attributeFilterVisitor;
                }
                else
                { 
                    ContainerFilterVisitor containerFilterVisitor = new ContainerFilterVisitor(expression);
                    return containerFilterVisitor;
                }
            }
            if (this.allowedExtensionMethods.Contains(expression.Method.Name))
            {
                MethodFilterVisitor methodFilterVisitor = new MethodFilterVisitor(expression);
                return methodFilterVisitor;
            }
            if (expression.Method.Name == "Subtree")
            {
                SubtreeFilterVisitor subtreeFilterVisitor = new SubtreeFilterVisitor(expression);
                return subtreeFilterVisitor;
            }
            if (expression.Method.Name == "Range")
            {
                RangeGroupFilterVisitor rangeFilterVisitor = new RangeGroupFilterVisitor(new List<MethodCallExpression>() { expression });
                return rangeFilterVisitor;
            }
            if (expression.Method.Name == "FirstOrDefault")
            {
                return Create(expression.Arguments[0]);
            }
            if (expression.Arguments.Count > 1 && expression.Arguments[1].NodeType == ExpressionType.NewArrayInit)
            {
                InGroupFilterVisitor inGroupFilterVisitor = new InGroupFilterVisitor(expression);
                return inGroupFilterVisitor;
            }
            if (expression.Method.Name == "get_Item")
            {
                PropertyFilterVisitor propertyFilterVisitor = new PropertyFilterVisitor(expression);
                return propertyFilterVisitor;
            }

            throw new NotSupportedException("The expression type is not supported.");
        }

        private FilterVisitor CreateFilterVisitor(BinaryExpression expression)
        {
            EqualFilterVisitor equalFilterVisitor = new EqualFilterVisitor(expression);
            return equalFilterVisitor;
        }

        private FilterVisitor CreateFilterVisitor(MemberExpression expression)
        {
            PropertyFilterVisitor propertyFilterVisitor = new PropertyFilterVisitor(expression);
            return propertyFilterVisitor;
        }

        private FilterVisitor CreateGroupFilterVisitor(BinaryExpression expression)
        {
            GroupExpressionParser groupExpressionParser = new GroupExpressionParser();
            List<Expression> expressions = groupExpressionParser.FlattenExpressions(expression);
            GroupFilterVisitorFactory groupFilterFactory = new GroupFilterVisitorFactory();
            FilterVisitor filterVisitor = groupFilterFactory.GetGroupFilterVisitor(expressions);
            return filterVisitor;
        }
    }
}