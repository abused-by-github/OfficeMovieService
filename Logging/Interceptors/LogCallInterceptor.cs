using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;
using Svitla.MovieService.Core.Helpers;
using Svitla.MovieService.Core.Logging;

namespace Svitla.MovieService.Logging.Interceptors
{
    public abstract class LogCallInterceptor : IInterceptor
    {
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
                    args[@params[i].Name] = invocation.Arguments[i];
                }

                Logger.LogMethodStart(verbosity, type, method, args, callId);

                try
                {
                    invocation.Proceed();
                    Logger.LogMethodEnd(verbosity, type, method, invocation.ReturnValue, true, callId);
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
