namespace commercetools.Sdk.Linq.Filter.Visitors
{
    public class SubtreePredicateVisitor : IPredicateVisitor
    {
        private readonly IPredicateVisitor id;

        public SubtreePredicateVisitor(IPredicateVisitor id)
        {
            this.id = id;
        }

        public string Render()
        {
            return $" subtree({this.id.Render()})";
        }
    }
}
