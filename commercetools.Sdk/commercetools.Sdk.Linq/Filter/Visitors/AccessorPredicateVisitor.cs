namespace commercetools.Sdk.Linq.Filter.Visitors
{
    public class AccessorPredicateVisitor : IPredicateVisitor
    {
        public AccessorPredicateVisitor(IPredicateVisitor current, IPredicateVisitor parent)
        {
            this.Current = current;
            this.Parent = parent;
        }

        public IPredicateVisitor Parent { get; }

        public IPredicateVisitor Current { get; }

        public string Render()
        {
            if (this.Parent != null && this.Current != null)
            {
                // TODO Find another way to fix the replace problem.
                // This happens when property Value is not rendered for attributes.
                return $"{this.Parent.Render()}.{this.Current.Render()}".Replace(".:", ":");
            }
            else if (this.Parent == null && this.Current != null)
            {
                return $"{this.Current.Render()}";
            }
            else
            {
                return $"{this.Parent?.Render()}";
            }
        }
    }
}
