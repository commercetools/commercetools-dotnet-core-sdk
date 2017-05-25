namespace commercetools.Messages
{
    /// <summary>
    /// MessageFactory
    /// </summary>
    public class MessageFactory
    {
        /// <summary>
        /// Creates a Message using JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        /// <returns>A object derived from Message, or null</returns>
        public static Message Create(dynamic data = null)
        {
            if (data == null || data.type == null)
            {
                return null;
            }

            switch ((string)data.type)
            {
                case "LineItemStateTransition":
                    return new LineItemStateTransitionMessage(data);
                case "CustomLineItemStateTransition":
                    return new CustomLineItemStateTransitionMessage(data);
                case "DeliveryAdded":
                    return new DeliveryAddedMessage(data);
                case "ParcelAddedToDelivery":
                    return new ParcelAddedToDeliveryMessage(data);
                case "ReturnInfoAdded":
                    return new ReturnInfoAddedMessage(data);
                case "OrderCreated":
                    return new OrderCreatedMessage(data);
                case "OrderImported":
                    return new OrderImportedMessage(data);
                case "OrderStateChanged":
                    return new OrderStateChangedMessage(data);
                case "OrderStateTransition":
                    return new OrderStateTransitionMessage(data);
                case "OrderCustomerEmailSet":
                    return new OrderCustomerEmailSetMessage(data);
                case "OrderShippingAddressSet":
                    return new OrderShippingAddressSetMessage(data);
                case "OrderBillingAddressSet":
                    return new OrderBillingAddressSetMessage(data);
                case "ProductCreated":
                    return new ProductCreatedMessage(data);
                case "ProductPublished":
                    return new ProductPublishedMessage(data);
                case "ProductUnpublished":
                    return new ProductUnpublishedMessage(data);
                case "ProductStateTransition":
                    return new ProductStateTransitionMessage(data);
                case "ProductSlugChanged":
                    return new ProductSlugChangedMessage(data);
                case "CategoryCreated":
                    return new CategoryCreatedMessage(data);
                case "CategorySlugChanged":
                    return new CategorySlugChangedMessage(data);
                case "PaymentCreated":
                    return new PaymentCreatedMessage(data);
                case "PaymentInteractionAdded":
                    return new PaymentInteractionAddedMessage(data);
                case "PaymentTransactionAdded":
                    return new PaymentTransactionAddedMessage(data);
                case "PaymentTransactionStateChanged":
                    return new PaymentTransactionStateChangedMessage(data);
                case "PaymentStatusStateTransition":
                    return new PaymentStatusStateTransitionMessage(data);
                case "CustomerCreated":
                    return new CustomerCreatedMessage(data);
                case "ReviewCreated":
                    return new ReviewCreatedMessage(data);
                case "ReviewStateTransition":
                    return new ReviewStateTransitionMessage(data);
                case "ReviewRatingSet":
                    return new ReviewRatingSetMessage(data);
                case "InventoryEntryDeleted":
                    return new InventoryEntryDeletedMessage(data);
            }

            return null;
        }
    }
}
