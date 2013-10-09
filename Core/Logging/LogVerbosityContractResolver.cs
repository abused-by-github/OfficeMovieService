﻿using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Svitla.MovieService.Core.Helpers;

namespace Svitla.MovieService.Core.Logging
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
            var logSettings = (LogAttribute) member.GetCustomAttributes(typeof (LogAttribute)).SingleOrDefault();
            property.Ignored = logVerbosity < logSettings.Get(s => s.Verbosity);

            return property;
        }
    }
}
