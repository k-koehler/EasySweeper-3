using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWinterface
{
    class DuplicateFloorException : Exception
    {
        public DuplicateFloorException() : base() { }
        public DuplicateFloorException(string message) : base(message) { }
        public DuplicateFloorException(string message, Exception inner) : base(message, inner) { }

        protected DuplicateFloorException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
