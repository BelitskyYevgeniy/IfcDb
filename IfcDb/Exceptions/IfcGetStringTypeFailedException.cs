namespace IfcDb.Exceptions
{
    public class IfcGetStringTypeFailedException : IfcParsingFailedException
    {
        public IfcGetStringTypeFailedException(string str) : base($"Can not get type of string. String '{str}'") { }
    }
}
