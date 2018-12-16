namespace commercetools.Sdk.Linq.Filter.Visitors
{
    public class RangePredicateVisitor : IPredicateVisitor
    {
        private readonly IPredicateVisitor from;
        private readonly IPredicateVisitor to;

        public RangePredicateVisitor(IPredicateVisitor from, IPredicateVisitor to)
        {
            this.from = from;
            this.to = to;
        }

        public string Render()
        {
            return $"({this.from.Render()} to {this.to.Render()})";
        }
    }
}
