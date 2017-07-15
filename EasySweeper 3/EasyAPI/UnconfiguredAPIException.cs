using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAPI
{
    class UnconfiguredAPIException : Exception
    {
        public UnconfiguredAPIException() : base("API Unconfigured, have you called ConfigureInstance?") { }
        public UnconfiguredAPIException(string message) : base(message) { }
        public UnconfiguredAPIException(string message, Exception inner) : base(message, inner) { }

        protected UnconfiguredAPIException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
