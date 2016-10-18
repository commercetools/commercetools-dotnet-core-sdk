using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;

using Newtonsoft.Json.Linq;

namespace commercetools.Common
{
    /// <summary>
    /// A collection of static methods for common tasks.
    /// </summary>
    public static class Helper
    {
        #region Enum

        /// <summary>
        /// Attempts to get an enum by the value of its EnumMember attribute.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="value">Value</param>
        /// <param name="t"></param>
        /// <returns>Enum with the matching EnumMember, or the default value for T if not found</returns>
        public static bool TryGetEnumByEnumMemberAttribute<T>(string value, out T t)
        {
            t = default(T);
            Type type = typeof(T);

            if (Nullable.GetUnderlyingType(typeof(T)) != null)
            {
                type = Nullable.GetUnderlyingType(typeof(T));
            }

            MemberInfo[] memberInfoList = type.GetFields();
            bool found = false;

            foreach (var memberInfo in memberInfoList)
            {
                EnumMemberAttribute[] attributes = (EnumMemberAttribute[])memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false);

                if (attributes != null && attributes.Length > 0 && attributes[0].Value.Equals(value))
                {
                    t = (T)Enum.Parse(type, memberInfo.Name);
                    found = true;
                    break;
                }
            }

            return found;
        }

        #endregion

        #region Object creation

        public delegate T ObjectActivator<T>(params object[] args);

        /// <summary>
        /// Object activator. Offers significantly improved performance over Activator.CreateInstance.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="constructor">Constructor to use</param>
        /// <returns>ObjectActivator</returns>
        public static ObjectActivator<T> GetActivator<T>(ConstructorInfo constructor)
        {
            Type type = constructor.DeclaringType;
            ParameterInfo[] parameterInfoList = constructor.GetParameters();
            ParameterExpression parameterExpression = Expression.Parameter(typeof(object[]), "args");
            Expression[] argsExpressionList = new Expression[parameterInfoList.Length];

            for (int i = 0; i < parameterInfoList.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type parameterType = parameterInfoList[i].ParameterType;
                Expression paramAccessorExpression = Expression.ArrayIndex(parameterExpression, index);
                Expression paramCastExpression = Expression.Convert(paramAccessorExpression, parameterType);
                argsExpressionList[i] = paramCastExpression;
            }

            NewExpression newExpression = Expression.New(constructor, argsExpressionList);
            LambdaExpression lambda = Expression.Lambda(typeof(ObjectActivator<T>), newExpression, parameterExpression);

            return (ObjectActivator<T>)lambda.Compile();
        }

        /// <summary>
        /// Gets a list from an array of JSON objects.
        /// </summary>
        /// <remarks>
        /// For instances of T to be created, T must have a constructor that accepts one parameter: "data" of type System.Object
        /// </remarks>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="jArray">Array of JSON objects</param>
        /// <returns>List of T, or null</returns>
        public static List<T> GetListFromJsonArray<T>(JArray jArray)
        {
            if (jArray == null || jArray.Count < 1)
            {
                return new List<T>();
            }

            List<T> list = new List<T>();

            Type type = typeof(T);
            ConstructorInfo myConstructorInfo = null;
            ConstructorInfo[] constructorInfoList = type.GetConstructors();

            foreach (ConstructorInfo constructorInfo in constructorInfoList)
            {
                ParameterInfo[] parameters = constructorInfo.GetParameters();

                if (parameters.Length == 1 && parameters[0].Name.Equals("data") && parameters[0].ParameterType == typeof(object))
                {
                    myConstructorInfo = constructorInfo;
                    break;
                }
            }

            if (myConstructorInfo != null)
            {
                Helper.ObjectActivator<T> activator = Helper.GetActivator<T>(myConstructorInfo);

                foreach (dynamic data in jArray)
                {
                    T listItem = activator(data);
                    list.Add(listItem);
                }
            }

            return list;
        }

        /// <summary>
        /// Gets a string list from an array of JValues.
        /// </summary>
        /// <param name="jArray">Array</param>
        /// <returns>List of strings</returns>
        public static List<string> GetStringListFromJsonArray(JArray jArray)
        {
            if (jArray == null || jArray.Count < 1)
            {
                return new List<string>();
            }

            List<string> list = new List<string>();
            var values = jArray.Children<JValue>();

            foreach (JValue value in values)
            {
                list.Add(value.ToString());
            }

            return list;
        }

        #endregion

        #region Utility

        /// <summary>
        /// Returns a properly encoded string for use in client requests.
        /// </summary>
        /// <param name="str">String to encode</param>
        /// <returns>URL encoded string</returns>
        public static string UrlEncode(string str)
        {
            str = HttpUtility.UrlEncode(str);
            str = str.Replace("(", "%28");
            str = str.Replace(")", "%29");
            return str;
        }

        #endregion
    }
}