using System;

namespace IfcDb.Exceptions
{
    public class IfcObjParsingFailedException : IfcParsingFailedException
    {
        public IfcObjParsingFailedException(string objStr) : base($"Failed to parse object. Object: '{objStr}'") { }
    }
}
