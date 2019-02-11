using System;
using System.Globalization;
using System.Linq.Expressions;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    // local variable string key = "c14"
    // key in c.Key = key
    // int minutes = 10
    // hardcoded
    // "c14" in c.Key = "c14"
    public class ConstantPredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        public int Priority { get; } = 4;

        public bool CanConvert(Expression expression)
        {
            if (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Method.Name == "valueOf")
                {
                    return true;
                }
            }

            return expression.NodeType == ExpressionType.Constant || IsVariable(expression);
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            if (expression.NodeType == ExpressionType.Constant)
            {
                string constantExpression = this.FormatConstantString(expression);
                return new ConstantPredicateVisitor(constantExpression);
            }

            var compiledValue = Expression.Lambda(expression, null).Compile().DynamicInvoke(null).ToString();
            if (expressionType(expression) == typeof(string))
            {
                compiledValue = compiledValue.WrapInQuotes();
            }

            return new ConstantPredicateVisitor(compiledValue);
        }

        private Type expressionType(Expression expression)
        {
            if (expression is MethodCallExpression)
            {
                return (expression as MethodCallExpression).Arguments[0].Type;
            }

            return (expression as MemberExpression).Type;
        }
        
        private static bool IsVariable(Expression expression)
        {
            if (expression.NodeType != ExpressionType.MemberAccess)
            {
                return false;
            }

            MemberExpression memberExpression = expression as MemberExpression;
            if (memberExpression?.Expression.NodeType == ExpressionType.Constant)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if the constant is double then format it with Invariant culture else return expression string
        /// </summary>
        /// <param name="expression">constant expression</param>
        /// <returns>formatted constant as string</returns>
        private string FormatConstantString(Expression expression)
        {
            string expressionString = expression.ToString();
            if (double.TryParse(expressionString, out var doubleConstant))
            {
                expressionString = string.Format(CultureInfo.InvariantCulture, "{0}", doubleConstant);
            }

            return expressionString;
        }
    }
}