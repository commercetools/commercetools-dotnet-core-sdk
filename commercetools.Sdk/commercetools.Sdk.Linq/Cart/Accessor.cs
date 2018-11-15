namespace commercetools.Sdk.Linq
{
    public class Accessor : ICartPredicateVisitor, IAccessorAppendable
    {
        private ICartPredicateVisitor parentAccessor;
        private readonly ICartPredicateVisitor currentAccessor;

        public Accessor(ICartPredicateVisitor currentAccessor, ICartPredicateVisitor parentAccessor)
        {
            this.currentAccessor = currentAccessor;
            this.parentAccessor = parentAccessor;
        }

        public void AppendAccessor(Accessor accessor)
        {
            if (this.parentAccessor == null)
            {
                this.parentAccessor = accessor;
            }
            else
            {
                if (this.parentAccessor is IAccessorAppendable accessorAppendablePredicate)
                { 
                    accessorAppendablePredicate.AppendAccessor(accessor);
                }
            }
        }

        public string Render()
        {
            if (parentAccessor == null && this.currentAccessor != null)
            {
                return this.currentAccessor.Render();
            }
            if (parentAccessor != null && this.currentAccessor != null)
            { 
                return $"{parentAccessor.Render()}.{this.currentAccessor.Render()}";
            }
            if (parentAccessor != null && this.currentAccessor == null)
            {
                return $"{parentAccessor.Render()}";
            }
            return string.Empty;
        }
    }
}
