using System.IO;

namespace commercetools.Sdk.Linq.Query.Visitors
{
    // name = "Peter"
    public class BinaryPredicateVisitor : IPredicateVisitor
    {
        private readonly IPredicateVisitor left;
        private readonly IPredicateVisitor right;
        private readonly string operatorSign;

        public BinaryPredicateVisitor(IPredicateVisitor left, string operatorSign, IPredicateVisitor right)
        {
            this.left = left;
            this.right = right;
            this.operatorSign = operatorSign;
        }

        public string Render()
        {
            string result;

            // if the operator is empty or null, then remove it to avoid double spaces
            if (string.IsNullOrEmpty(this.operatorSign))
            {
                result = $"{this.left.Render()} {this.right.Render()}";
            }
            else
            {
                result = $"{this.left.Render()} {this.operatorSign} {this.right.Render()}";
            }

            // It can happen that the right predicate is an empty string, hence we trim the white space.
            result = result.TrimEnd();
            return result;
        }
    }
}