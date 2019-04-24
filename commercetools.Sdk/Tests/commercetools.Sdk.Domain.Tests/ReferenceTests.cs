using commercetools.Sdk.Domain.Carts;
using Xunit;

namespace commercetools.Sdk.Domain.Tests
{
    public class ReferenceTests
    {
        [Fact]
        public void ResourceToReference()
        {
            var cart = new Cart();
            var reference = cart.ToReference();

            Assert.IsType<Reference<Cart>>(reference);
            Assert.IsType<Cart>(((Reference<Cart>) reference).Obj);
        }

        [Fact]
        public void ReferenceToReference()
        {
            var reference = new Reference<Cart>();

            Assert.IsType<Reference<Cart>>(reference.ToReference());
        }
    }
}
