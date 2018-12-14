namespace commercetools.Sdk.Linq.Query.Visitors
{
    public class BinaryLogicalPredicateVisitor : IPredicateVisitor
    {
        private readonly IPredicateVisitor left;
        private readonly IPredicateVisitor right;
        private readonly string operatorSign;

        public BinaryLogicalPredicateVisitor(IPredicateVisitor left, string operatorSign, IPredicateVisitor right)
        {
            this.left = left;
            this.right = right;
            this.operatorSign = operatorSign;
        }

        public string Render()
        {
            return $"{this.left.Render()} {this.operatorSign} {this.right.Render()}";
        }
    }
}
