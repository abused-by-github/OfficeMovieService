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

        public static void LogMethodStart(Verbosity verbosity, string type, string method, Dictionary<string, object> args, string callId = "")
        {
            Task.Run(() =>
            {
                var format = BeforeMethodCallLog;
                if (!string.IsNullOrEmpty(callId))
                {
                    format = "Call <" + callId + "> " + format;
                }
                var argsJson = Serialize(verbosity, args);
                nLog.Info(string.Format(format, method, type, argsJson));
            });
        }

        public static void LogMethodEnd(Verbosity verbosity, string type, string method, object result, bool isSuccess, string callId = "")
        {
            Task.Run(() =>
            {
                var format = isSuccess ? AfterMethodSuccessCallLog : AfterMethodUnSuccessCallLog;
                if (!string.IsNullOrEmpty(callId))
                {
                    format = "Call <" + callId + "> " + format;
                }
                var resultJson = Serialize(verbosity, result);
                nLog.Info(string.Format(format, method, type, resultJson));
            });
        }

        private static string Serialize(Verbosity verbosity, object obj)
        {
            string result;

            var serializer = new JsonSerializer();
            serializer.ContractResolver = new LogVerbosityContractResolver(verbosity);
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                result = writer.ToString();
            }

            return result;
        }
    }
}
