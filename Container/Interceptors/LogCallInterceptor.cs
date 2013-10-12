using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using Svitla.MovieService.Core.Helpers;
using Svitla.MovieService.Core.Logging;

namespace Svitla.MovieService.Container.Interceptors
{
    public abstract class LogCallInterceptor : IInterceptor
    {
        private static readonly object logSkipped = new { LogSkipped = "Due to verbosity" };
        private readonly Verbosity verbosity;

        protected LogCallInterceptor(Verbosity verbosity)
        {
            this.verbosity = verbosity;
        }

        public void Intercept(IInvocation invocation)
        {
            if (IsMethodLogable(invocation.Method))
            {
                //We need some tag which allow us to match method start and end in log.
                var callId = Guid.NewGuid().ToString();
                var method = invocation.Method.Name;
                var type = invocation.TargetType.FullName;

                Dictionary<string, object> args = new Dictionary<string, object>();
                var @params = invocation.Method.GetParameters();
                for (var i = 0; i < @params.Length; ++i)
                {
                    var paramVerbosity = @params[i].GetCustomAttribute<LogAttribute>().Get(a => a.Verbosity);
                    var islogable = verbosity >= paramVerbosity;
                    args[@params[i].Name] = islogable ? invocation.Arguments[i] : logSkipped;
                }

                Logger.LogMethodStart(verbosity, type, method, args, callId);

                try
                {
                    invocation.Proceed();
                    var returnVerbosity = invocation.Method.ReturnTypeCustomAttributes
                        .GetCustomAttributes(typeof(LogAttribute), false)
                        .Cast<LogAttribute>()
                        .FirstOrDefault()
                        .Get(a => a.Verbosity);
                    object returnValue = verbosity >= returnVerbosity ? invocation.ReturnValue : logSkipped;
                    Logger.LogMethodEnd(verbosity, type, method, returnValue, true, callId);
                }
                catch (Exception e)
                {
                    Logger.LogMethodEnd(verbosity, type, method, e, false, callId);
                    throw;
                }
            }
            else
            {
                invocation.Proceed();
            }
        }

        //TODO: proxy shouldn't be created for not logable methods at all
        private bool IsMethodLogable(MethodInfo method)
        {
            var result = method.IsPublic;
            if (result)
            {
                var methodVerbosity = method.GetCustomAttribute<LogAttribute>().Get(a => a.Verbosity);
                result = verbosity >= methodVerbosity;
            }
            return result;
        }
    }
}
