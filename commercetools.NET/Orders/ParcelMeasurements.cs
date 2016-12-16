using System;

using Newtonsoft.Json;

namespace commercetools.Orders
{
    /// <summary>
    /// Information regarding the dimensions of a parcel.
    /// </summary>
    /// <see href="http://dev.commercetools.com/http-api-projects-orders.html#parcelmeasurements"/>
    public class ParcelMeasurements
    {
        #region Properties

        [JsonProperty(PropertyName = "heightInMillimeter")]
        public int? HeightInMillimeter { get; private set; }

        [JsonProperty(PropertyName = "lengthInMillimeter")]
        public int? LengthInMillimeter { get; private set; }

        [JsonProperty(PropertyName = "widthInMillimeter")]
        public int? WidthInMillimeter { get; private set; }

        [JsonProperty(PropertyName = "weightInGram")]
        public int? WeightInGram { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance with JSON data from an API response.
        /// </summary>
        /// <param name="data">JSON object</param>
        public ParcelMeasurements(dynamic data = null)
        {
            if (data == null)
            {
                return;
            }

            this.HeightInMillimeter = data.heightInMillimeter;
            this.LengthInMillimeter = data.lengthInMillimeter;
            this.WidthInMillimeter = data.widthInMillimeter;
            this.WeightInGram = data.weightInGram;
        }

        #endregion
    }
}
