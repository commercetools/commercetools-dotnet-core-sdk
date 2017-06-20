namespace commercetools.CartDiscounts
{
    public class CartDiscountValueFactory
    {
        /// <summary>
        /// Creates a CartDiscountValue using JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        /// <returns>A object derived from CartDiscountValue, or null</returns>
        public static CartDiscountValue Create(dynamic data = null)
        {
            if (data == null || data.type == null)
            {
                return null;
            }

            switch ((string)data.type)
            {
                case "relative":
                    return new RelativeCartDiscountValue(data);
                case "absolute":
                    return new AbsoluteCartDiscountValue(data);
                case "giftLineItem":
                    return new GiftLineItemCartDiscountValue(data); 
            }

            return null;
        }
    }
}
