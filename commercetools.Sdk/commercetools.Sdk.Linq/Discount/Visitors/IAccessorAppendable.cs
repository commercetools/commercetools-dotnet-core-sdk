namespace commercetools.Sdk.Linq.Discount.Visitors
{
    public interface IAccessorAppendable
    {
        void AppendAccessor(AccessorPredicateVisitor accessor);
    }
}