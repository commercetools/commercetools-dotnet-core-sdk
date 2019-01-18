namespace commercetools.Sdk.Linq.Query.Visitors
{
    // variants(not(attributes(name="attribute-name")))
    public class ContainerPredicateVisitor : IPredicateVisitor
    {
        public ContainerPredicateVisitor(IPredicateVisitor inner, IPredicateVisitor parent)
        {
            this.Inner = inner;
            this.Parent = parent;
        }

        public IPredicateVisitor Parent { get; }

        public IPredicateVisitor Inner { get; }

        public string Render()
        {
            return this.Parent != null ? $"{this.Parent.Render()}({this.Inner?.Render()})" : this.Inner?.Render();
        }
    }
}
