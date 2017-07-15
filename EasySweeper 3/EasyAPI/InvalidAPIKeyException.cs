using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAPI
{
    class InvalidAPIKeyException : Exception
    {
        public InvalidAPIKeyException(Guid key) : base() { Key = key; }
        public InvalidAPIKeyException(Guid key, string message) : base(message) { Key = key; }
        public InvalidAPIKeyException(Guid key, string message, Exception inner) : base(message, inner) { Key = key; }

        protected InvalidAPIKeyException(Guid key, System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
        { Key = key; }

        public Guid Key { get; }

        public override string ToString()
        {
            return base.ToString() + $"\n API Key Passed: {Key}";
        }
    }
}
