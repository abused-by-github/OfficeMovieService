using System;

namespace Svitla.MovieService.Core.Helpers
{
    public static class ObjectExtensions
    {
        public static TOut Get<TIn, TOut>(this TIn obj, Func<TIn, TOut> get)
            where TIn : class
        {
            return obj == null ? default (TOut) : get(obj);
        }
    }
}
