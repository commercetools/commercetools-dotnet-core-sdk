using System.IO;
using System.Reflection;

using log4net;
using log4net.Repository;

namespace commercetools.Common
{
    /// <summary>
    /// A simple log module using log4net.
    /// </summary>
    public class Logger
    {
        private static bool _configurationAttempted = false;
        private static ILog _log;
        private static string _configurationFile;       
        
        public string ConfigurationFile {
            get {
                return _configurationFile;
            }
            set {
                _configurationFile = value;
                _log = null;
                _configurationAttempted = false;
            }
        }

        /// <summary>
        /// ILog
        /// </summary>
        private static ILog Log
        {
            get
            {
                if (!_configurationAttempted)
                {
                    _configurationAttempted = true;
                    try
                    {
                        Assembly assembly = Assembly.GetCallingAssembly() ?? Assembly.GetEntryAssembly();
                        if (assembly != null && !LogManager.GetRepository(assembly).Configured)
                        {
                            if (string.IsNullOrEmpty(_configurationFile))
                            {
                                //attempt to load the config from the default location                                
                                log4net.Config.XmlConfigurator.Configure(LogManager.GetRepository(assembly));
                                _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                            } else
                            {
                                //load from the specified log file
                                ILoggerRepository logRepository = LogManager.GetRepository(assembly);
                                log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo(_configurationFile));
                                _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                            }
                        }
                    }
                    catch
                    {
                        _log = null;
                    }
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
            if (Log != null)
            {
                Log.Info(message);
            }
        }

        /// <summary>
        /// Logs a warning
        /// </summary>
        /// <param name="message">Message</param>
        public static void LogWarning(string message)
        {
            if (Log != null)
            {
                Log.Warn(message);
            }
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">Error message</param>
        public static void LogError(string message)
        {
            if (Log != null)
            {
                Log.Error(message);
            }
        }
    }
}
