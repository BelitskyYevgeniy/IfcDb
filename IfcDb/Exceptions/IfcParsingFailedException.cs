using System;

namespace IfcDb.Exceptions
{
    public class IfcParsingFailedException : Exception
    {
        public IfcParsingFailedException(string message) : base(message) { }
    }
}
