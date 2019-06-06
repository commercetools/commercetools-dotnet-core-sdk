using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using commercetools.Sdk.Domain.Payments;
using commercetools.Sdk.Domain.Validation.Attributes;

namespace commercetools.Sdk.Domain.Orders.UpdateActions
{
    public class SetParcelMeasurementsUpdateAction : UpdateAction<Order>
    {
        public string Action => "setParcelMeasurements";
        [Required]
        public string ParcelId { get; set; }
        public ParcelMeasurements Measurements { get; set; }
    }
}
