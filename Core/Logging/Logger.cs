using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;

namespace Svitla.MovieService.Core.Logging
{
    public static class Logger
    {
        private const string LoggerName = "MovieServiceLogger";
        private const string BeforeMethodCallLog = "Method {0} of type {1} is being called with parameters:\n{2}";
        private const string AfterMethodSuccessCallLog = "Method {0} of type {1} has been called with result:\n{2}";
        private const string AfterMethodUnSuccessCallLog = "Method {0} of type {1} has been called with exception:\n{2}";

        private static readonly NLog.Logger nLog = LogManager.GetLogger(LoggerName);

        public static void Log(Exception e)
        {
            nLog.ErrorException("", e);
        }

        public static void LogMethodStart(Verbosity verbosity, string type, string method, Dictionary<string, object> args)
        {
            Task.Run(() =>
            {
                string argsJson;
                var serializer = new JsonSerializer();
                serializer.ContractResolver = new LogVerbosityContractResolver(verbosity);
                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, args);
                    argsJson = writer.ToString();
                }

                nLog.Info(string.Format(BeforeMethodCallLog, method, type, argsJson));
            }).Start();
        }

        public static void LogMethodEnd(Verbosity verbosity, string type, string method, object result, bool isSuccess)
        {
            Task.Run(() =>
            {
                string resultJson;
                var serializer = new JsonSerializer();
                serializer.ContractResolver = new LogVerbosityContractResolver(verbosity);
                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, result);
                    resultJson = writer.ToString();
                }

                var message = isSuccess ? AfterMethodSuccessCallLog : AfterMethodUnSuccessCallLog;
                nLog.Info(string.Format(message, method, type, resultJson));
            }).Start();
        }
    }
}
