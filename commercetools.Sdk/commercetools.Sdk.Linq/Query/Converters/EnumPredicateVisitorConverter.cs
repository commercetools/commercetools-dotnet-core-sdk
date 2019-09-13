using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using commercetools.Sdk.Linq.Query.Visitors;

namespace commercetools.Sdk.Linq.Query.Converters
{
    public class EnumPredicateVisitorConverter : IQueryPredicateVisitorConverter
    {
        public int Priority { get; } = 3;

        public bool CanConvert(Expression expression)
        {
            return expression.NodeType == ExpressionType.Constant && typeof(Enum).IsAssignableFrom(expression.Type);
        }

        public IPredicateVisitor Convert(Expression expression, IPredicateVisitorFactory predicateVisitorFactory)
        {
            string constantExpression = FormatEnumString(expression);
            return new ConstantPredicateVisitor(constantExpression);
        }

        /// <summary>
        /// return value of Enum as value of Description custom Attribute Wrapped In Quotes
        /// </summary>
        /// <param name="expression">constant expression</param>
        /// <returns>formatted enum as string</returns>
        private static string FormatEnumString(Expression expression)
        {
            string expressionString = GetEnumDescription(expression);
            expressionString = expressionString.WrapInQuotes();
            return expressionString;
        }

        private static string GetEnumDescription(Expression expression)
        {
            var enumValue = expression.ToString();
            FieldInfo fi = expression.Type.GetField(enumValue);
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }

            return enumValue;
        }
    }
}