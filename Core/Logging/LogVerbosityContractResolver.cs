using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using MovieService.Core.Helpers;

namespace MovieService.Core.Logging
{
    class LogVerbosityContractResolver : DefaultContractResolver
    {
        private readonly Verbosity logVerbosity;

        public LogVerbosityContractResolver(Verbosity logVerbosity) : base(false)
        {
            this.logVerbosity = logVerbosity;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (!property.Ignored)
            {
                var propertyInfo = member as PropertyInfo;
                if (propertyInfo != null)
                {
                    property.Ignored = !propertyInfo.GetMethod.IsPublic;
                }
            }

            if (!property.Ignored)
            {
                var fieldInfo = member as FieldInfo;
                if (fieldInfo != null)
                {
                    property.Ignored = !fieldInfo.IsPublic;
                }
            }

            if (!property.Ignored)
            {
                var logSettings = (LogAttribute) member
                    .GetCustomAttributes(typeof (LogAttribute)).SingleOrDefault();
                property.Ignored = logVerbosity < logSettings.Get(s => s.Verbosity);
            }
            return property;
        }
    }
}
