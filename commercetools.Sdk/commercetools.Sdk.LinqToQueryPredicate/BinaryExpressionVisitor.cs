using System.Collections.Generic;

namespace commercetools.Sdk.LinqToQueryPredicate
{
    // TODO Change name
    public class BinaryExpressionVisitor
    {
        public string left;
        public string right;
        public string operatorSign;
        public List<string> parentList = new List<string>();

        public override string ToString()
        {
            string result = $"{left.ToCamelCase()} {operatorSign.ToCamelCase()} {right}";
            return QueryPredicateExpressionVisitor.Visit(result, this.parentList);
        }
    }
}