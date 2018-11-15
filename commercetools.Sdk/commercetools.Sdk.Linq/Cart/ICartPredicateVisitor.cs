namespace commercetools.Sdk.Linq
{
    public interface ICartPredicateVisitor
    {
        string Render();
        void AppendAccessor(Accessor accessor);
    }
}