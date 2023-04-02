namespace IfcDb.Exceptions
{
    public class IfcObjAttributeListParsingFailedException : IfcParsingFailedException
    {
        public IfcObjAttributeListParsingFailedException(string attributeListStr) : base($"Failed to parse attribute list. Attributes: '{attributeListStr}'") { }
    }
}
