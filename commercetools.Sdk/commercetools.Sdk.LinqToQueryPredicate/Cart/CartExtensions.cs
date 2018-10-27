using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Carts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace commercetools.Sdk.Linq.Extensions.Carts
{
    public static class CartExtensions
    {
        public static int LineItemCount(this Cart source, Expression<Func<LineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static int LineItemCount(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static int CustomLineItemCount(this Cart source, Expression<Func<CustomLineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static int CustomLineItemCount(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static Money LineItemTotal(this Cart source, Expression<Func<LineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static Money LineItemTotal(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static Money CustomLineItemTotal(this Cart source, Expression<Func<CustomLineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static Money CustomLineItemTotal(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static Money LineItemNetTotal(this Cart source, Expression<Func<LineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static Money LineItemNetTotal(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static Money CustomLineItemNetTotal(this Cart source, Expression<Func<CustomLineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static Money CustomLineItemNetTotal(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static Money LineItemGrossTotal(this Cart source, Expression<Func<LineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static Money LineItemGrossTotal(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static Money CustomLineItemGrossTotal(this Cart source, Expression<Func<CustomLineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static Money CustomLineItemGrossTotal(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static bool LineItemExists(this Cart source, Expression<Func<LineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static bool LineItemExists(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }

        public static bool ForAllLineItems(this Cart source, Expression<Func<LineItem, bool>> parameter)
        {
            throw new NotImplementedException();
        }

        public static bool ForAllLineItems(this Cart source, bool parameter)
        {
            throw new NotImplementedException();
        }
    }
}
