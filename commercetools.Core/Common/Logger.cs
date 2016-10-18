using System;
using System.Reflection;
using System.IO;
using System.Net;

using log4net;

using Newtonsoft.Json;

namespace commercetools.Common
{
    /// <summary>
    /// A simple log module using log4net.
    /// </summary>
    public class Logger
    {
        private static ILog _log;

        private static ILog Log
        {
            get
            {
                if (_log == null)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                }

                return _log;
            }
        }

        /// <summary>
        /// Logs information
        /// </summary>
        /// <param name="message">Message</param>
        public static void LogInfo(string message)
        {
            Log.Info(message);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">Error message</param>
        public static void LogError(string message)
        {
            Log.Error(message);
        }

        /// <summary>
        /// Logs a WebException
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="ex">WebException</param>
        public static void LogWebException(string message, WebException ex)
        {
            Log.Error(message);
            Log.Error(ex);

            if (ex.Response != null)
            {
                using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    dynamic response = JsonConvert.DeserializeObject(reader.ReadToEnd());

                    if (response != null)
                    {
                        if (response.message != null)
                        {
                            Log.Error(string.Concat("Response message: ", response.message));
                            Console.WriteLine(string.Concat("Response message: ", response.message));
                        }

                        Log.Error(string.Concat("Full response: ", response.ToString()));
                        Console.WriteLine(string.Concat("Full response: ", response.ToString()));
                    }
                }
            }
        }
    }
}