using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
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
            Dictionary<string, object> args = new Dictionary<string, object>();
            var @params = invocation.Method.GetParameters();
            for (var i = 0; i < @params.Length; ++i)
            {
                args[@params[i].Name] = invocation.Arguments[i];
            }
            Logger.LogMethodStart(verbosity, invocation.TargetType.FullName, invocation.Method.Name, args);

            try
            {
                invocation.Proceed();
                Logger.LogMethodEnd(verbosity, invocation.TargetType.FullName, invocation.Method.Name, invocation.ReturnValue, true);
            }
            catch (Exception e)
            {
                Logger.LogMethodEnd(verbosity, invocation.TargetType.FullName, invocation.Method.Name, e, false);
                throw;
            }
        }
    }
}
