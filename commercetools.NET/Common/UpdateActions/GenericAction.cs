using System.Collections.Generic;

using commercetools.Common.Converters;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Common.UpdateActions
{
    /// <summary>
    /// A generic update action class used for actions that have not yet been implemented.
    /// </summary>
    [JsonConverter(typeof(GenericActionConverter))]
    public class GenericAction : UpdateAction
    {
        #region Properties

        /// <summary>
        /// List of properties.
        /// </summary>
        public Dictionary<string, JToken> Properties { get; private set; }

        /// <summary>
        /// Gets or sets the value for a property.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        public object this[string propertyName]
        {
            get
            {
                return GetProperty(propertyName);
            }
            set
            {
                SetProperty(propertyName, value);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="action">Action name</param>
        public GenericAction(string action)
        {
            this.Action = action;
            this.Properties = new Dictionary<string, JToken>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets a property.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <param name="value">Value</param>
        public void SetProperty(string propertyName, object value)
        {
            JToken jToken;

            if (value is JToken)
            {
                jToken = (JToken)value;
            }
            else
            {
                jToken = JToken.FromObject(value, new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });
            }

            if (this.Properties.ContainsKey(propertyName))
            {
                this.Properties[propertyName] = jToken;
            }
            else
            {
                this.Properties.Add(propertyName, jToken);
            }
        }

        /// <summary>
        /// Gets a property.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>JToken, or null if not set</returns>
        public JToken GetProperty(string propertyName)
        {
            if (this.Properties.ContainsKey(propertyName))
            {
                return this.Properties[propertyName];
            }

            return null;
        }

        /// <summary>
        /// Checks if there are any values in this instance.
        /// </summary>
        /// <returns>True if there are values, false otherwise</returns>
        public bool IsEmpty()
        {
            return (this.Properties == null || this.Properties.Count < 1);
        }

        #endregion
    }
}
