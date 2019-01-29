﻿using commercetools.Sdk.Linq.Discount;
using System;
using System.Linq.Expressions;
using commercetools.Sdk.Registration;

namespace commercetools.Sdk.Domain.ProductDiscounts
{
    public class ProductDiscountDraft : IDraft<ProductDiscount>
    {
        public LocalizedString Name { get; set; }
        public LocalizedString Description { get; set; }
        public ProductDiscountValue Value { get; set; }
        public string Predicate { get; set; }
        public string SortOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }

        public void SetPredicate(Expression<Func<Product, bool>> expression)
        {
            this.Predicate = ServiceLocator.Current.GetService<IDiscountPredicateExpressionVisitor>().Render(expression);
        }
    }
}