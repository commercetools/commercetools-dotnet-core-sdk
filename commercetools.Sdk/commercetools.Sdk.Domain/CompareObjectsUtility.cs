using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace commercetools.Sdk
{
    public class CompareObjectsUtility
    {
        public static bool CompareObjects(object inputObjectA, object inputObjectB, string[] ignorePropertiesList)
        {
            bool areObjectsEqual = true;
            //check if both objects are not null before starting comparing children
            if (inputObjectA != null && inputObjectB != null)
            {
                //create variables to store object values
                object value1, value2;

                PropertyInfo[] properties =
                    inputObjectA.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                //get all public properties of the object using reflection
                foreach (PropertyInfo propertyInfo in properties)
                {
                    //if it is not a readable property or if it is a ignorable property
                    //ingore it and move on
                    if (!propertyInfo.CanRead || ignorePropertiesList.Contains(propertyInfo.Name))
                        continue;

                    //get the property values of both the objects
                    value1 = propertyInfo.GetValue(inputObjectA, null);
                    value2 = propertyInfo.GetValue(inputObjectB, null);

                    // if the objects are primitive types such as (integer, string etc)
                    //that implement IComparable, we can just directly try and compare the value
                    if (CanDirectlyCompare(propertyInfo.PropertyType))
                    {
                        //compare the values
                        if (!CompareValues(value1, value2))
                        {
                            areObjectsEqual = false;
                            break;
                        }
                    }
                    //if the property is a collection (or something that implements IEnumerable)
                    //we have to iterate through all items and compare values
                    else if (IsEnumerableType(propertyInfo.PropertyType))
                    {
                        if (!CompareEnumerations(value1, value2, ignorePropertiesList))
                        {
                            areObjectsEqual = false;
                            break;
                        }
                    }
                    //if it is a class object, call the same function recursively again
                    else if (propertyInfo.PropertyType.IsClass)
                    {
                        if (!CompareObjects(propertyInfo.GetValue(inputObjectA, null),
                            propertyInfo.GetValue(inputObjectB, null), ignorePropertiesList))
                        {
                            areObjectsEqual = false;
                            break;
                        }
                    }
                    else
                    {
                        areObjectsEqual = false;
                        break;
                    }
                }
            }
            else
                areObjectsEqual = false;

            return areObjectsEqual;
        }

        /// <summary>
        /// Compares two values and returns if they are the same.
        /// </summary>
        private static bool CompareValues(object value1, object value2)
        {
            bool areValuesEqual = true;
            IComparable selfValueComparer = value1 as IComparable;

            // one of the values is null
            if (value1 == null && value2 != null || value1 != null && value2 == null)
                areValuesEqual = false;
            else if (selfValueComparer != null && selfValueComparer.CompareTo(value2) != 0)
                areValuesEqual = false;
            else if (!object.Equals(value1, value2))
                areValuesEqual = false;

            return areValuesEqual;
        }

        private static bool CompareEnumerations(object value1, object value2, string[] ignorePropertiesList)
        {
            // if one of the values is null, no need to proceed return false;
            if (value1 == null && value2 != null || value1 != null && value2 == null)
                return false;
            else if (value1 != null && value2 != null)
            {
                IEnumerable<object> enumValue1, enumValue2;
                enumValue1 = ((IEnumerable) value1).Cast<object>();
                enumValue2 = ((IEnumerable) value2).Cast<object>();

                // if the items count are different return false
                if (enumValue1.Count() != enumValue2.Count())
                    return false;
                // if the count is same, compare individual item
                else
                {
                    object enumValue1Item, enumValue2Item;
                    Type enumValue1ItemType;
                    for (int itemIndex = 0; itemIndex < enumValue1.Count(); itemIndex++)
                    {
                        enumValue1Item = enumValue1.ElementAt(itemIndex);
                        enumValue2Item = enumValue2.ElementAt(itemIndex);
                        enumValue1ItemType = enumValue1Item.GetType();
                        if (CanDirectlyCompare(enumValue1ItemType))
                        {
                            if (!CompareValues(enumValue1Item, enumValue2Item))
                                return false;
                        }
                        else if (!CompareObjects(enumValue1Item, enumValue2Item, ignorePropertiesList))
                            return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether value instances of the specified type can be directly compared.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if this value instances of the specified type can be directly compared; otherwise, <c>false</c>.
        /// </returns>
        private static bool CanDirectlyCompare(Type type)
        {
            return typeof(IComparable).IsAssignableFrom(type) || type.IsPrimitive || type.IsValueType;
        }

        private static bool IsEnumerableType(Type type)
        {
            return (typeof(IEnumerable).IsAssignableFrom(type));
        }
    }
}
