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
            // It can happen that the right predicate is an empty string, hence we trim the white space.
            string result = $"{this.left.Render()} {this.operatorSign} {this.right.Render()}".TrimEnd();
            return result/*.Replace("  ", " ")*/;
        }
    }
}
