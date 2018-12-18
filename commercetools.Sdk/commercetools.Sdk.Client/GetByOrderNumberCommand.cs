﻿using System.Collections.Generic;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Orders;
using commercetools.Sdk.Domain.Query;

namespace commercetools.Sdk.Client
{
    public class GetByOrderNumberCommand : GetCommand<Order>
    {
        public GetByOrderNumberCommand(string orderNumber)
        {
            this.Init(orderNumber);
        }

        public GetByOrderNumberCommand(string orderNumber, List<Expansion<Order>> expand)
            : base(expand)
        {
            this.Init(orderNumber);
        }

        public GetByOrderNumberCommand(string orderNumber, List<Expansion<Order>> expand, IAdditionalParameters<Order> additionalParameters)
            : base(expand, additionalParameters)
        {
            this.Init(orderNumber);
        }

        public GetByOrderNumberCommand(string orderNumber, IAdditionalParameters<Order> additionalParameters)
            : base(additionalParameters)
        {
            this.Init(orderNumber);
        }

        private void Init(string orderNumber)
        {
            this.ParameterKey = Parameters.OrderNumber;
            this.ParameterValue = orderNumber;
        }
    }
}