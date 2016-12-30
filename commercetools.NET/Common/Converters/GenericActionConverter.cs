using System;

using commercetools.Common.UpdateActions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace commercetools.Common.Converters
{
    /// <summary>
    /// Custom converter for the GenericAction class.
    /// </summary>
    public class GenericActionConverter : JsonConverter
    {
        /// <summary>
        /// CanRead
        /// </summary>
        public override bool CanRead
        {
            get 
            { 
                return false; 
            }
        }

        /// <summary>
        /// CanConvert
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <returns>True if the type is GenericAction, false otherwise</returns>
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(GenericAction));
        }

        /// <summary>
        /// ReadJson (not implemented)
        /// </summary>
        /// <param name="reader">JsonReader</param>
        /// <param name="objectType">Object type</param>
        /// <param name="existingValue">Existing value</param>
        /// <param name="serializer">Serializer</param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// WriteJson
        /// </summary>
        /// <param name="writer">JsonWriter</param>
        /// <param name="value">Value</param>
        /// <param name="serializer">JsonSerializer</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is GenericAction)
            {
                GenericAction genericAction = (GenericAction)value;

                JObject jObject = new JObject();
                jObject.Add("action", genericAction.Action);

                foreach (string key in genericAction.Properties.Keys)
                {
                    jObject.Add(key, genericAction.Properties[key]);
                }

                jObject.WriteTo(writer);
            }
        }
    }
}
