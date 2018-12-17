namespace commercetools.Sdk.Linq.Discount.Visitors
{
    public class AccessorPredicateVisitor : IPredicateVisitor, IAccessorAppendable
    {
        private readonly IPredicateVisitor currentAccessor;
        private IPredicateVisitor parentAccessor;

        public AccessorPredicateVisitor(IPredicateVisitor currentAccessor, IPredicateVisitor parentAccessor)
        {
            this.currentAccessor = currentAccessor;
            this.parentAccessor = parentAccessor;
        }

        public void AppendAccessor(AccessorPredicateVisitor accessor)
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
            if (this.parentAccessor == null && this.currentAccessor != null)
            {
                return this.currentAccessor.Render();
            }

            if (this.parentAccessor != null && this.currentAccessor != null)
            {
                return $"{this.parentAccessor.Render()}.{this.currentAccessor.Render()}";
            }

            if (this.parentAccessor != null && this.currentAccessor == null)
            {
                return this.parentAccessor.Render();
            }

            return string.Empty;
        }
    }
}