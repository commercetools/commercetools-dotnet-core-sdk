﻿using System;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    public class MethodPredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        public int Priority { get; } = 3;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (Mapping.AllowedMethods.ContainsKey(methodCallExpression.Method.Name))
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

            string methodName = Mapping.AllowedMethods[methodCallExpression.Method.Name];

            var memberExpression = methodCallExpression.Arguments[0] as MemberExpression;
            string currentName = memberExpression.Member.Name;
            var callerParent = predicateVisitorFactory.Create(memberExpression.Expression);

            ConstantPredicateVisitor constant = new ConstantPredicateVisitor(currentName);

            // c.Key.In("c14", "c15")
            if (methodCallExpression.Arguments.Count > 1)
            {
                IPredicateVisitor methodArguments = predicateVisitorFactory.Create(methodCallExpression.Arguments[1]);
                var inner = new BinaryPredicateVisitor(constant, methodName, methodArguments);

                return CombinePredicates(callerParent, inner);
            }

            // c.Key.IsDefined()
            var binaryPredicateVisitor = new BinaryPredicateVisitor(constant, methodName, new ConstantPredicateVisitor(string.Empty));
            return CombinePredicates(callerParent, binaryPredicateVisitor);
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
    }
}
