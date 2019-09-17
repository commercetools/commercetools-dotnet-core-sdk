using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace commercetools.Sdk.Linq
{
    public static class Mapping
    {
        public const string And = "and";
        public const string Or = "or";
        public const string Not = "not";

        public static Dictionary<ExpressionType, string> ComparisonOperators { get; } = new Dictionary<ExpressionType, string>()
        {
            { ExpressionType.Equal, "=" },
            { ExpressionType.LessThan, "<" },
            { ExpressionType.GreaterThan, ">" },
            { ExpressionType.LessThanOrEqual, "<=" },
            { ExpressionType.GreaterThanOrEqual, ">=" },
            { ExpressionType.NotEqual, "!=" }
        };

        public static Dictionary<string, string> AllowedMethods { get; } = new Dictionary<string, string>()
        {
            { "In", "in" },
            { "NotIn", "not in" },
            { "Contains", "contains" },
            { "ContainsAll", "contains all" },
            { "ContainsAny", "contains any" },
            { "IsEmpty", "is empty" },
            { "IsNotEmpty", "is not empty" },
            { "IsDefined", "is defined" },
            { "IsNotDefined", "is not defined" },
            { "IsIn", "=" },
            { "IsNotIn", "!=" }
        };

        public static Dictionary<ExpressionType, string> LogicalOperators { get; } = new Dictionary<ExpressionType, string>()
        {
            { ExpressionType.And, And },
            { ExpressionType.AndAlso, And },
            { ExpressionType.Or, Or },
            { ExpressionType.OrElse, Or }
        };

        public static IEnumerable<string> DateTimeCustomMethods { get; } = new List<string>()
        {
            "AsDateTimeAttribute",
            "AsDateAttribute",
            "AsTimeAttribute"
        };

        public static string CastMethodStart { get; } = "To";

        public static string GetOperator(ExpressionType expressionType, Dictionary<ExpressionType, string> mapping)
        {
            if (mapping.ContainsKey(expressionType))
            {
                return mapping[expressionType];
            }

            throw new NotSupportedException();
        }
    }
}
