namespace IfcDb.Models
{

    public enum IfcAttributeType : int
    {
        Null = 1,
        Derive,
        Integer,
        Real,
        String,
        EntityInstanceName,
        Enumeration,
        List,
        Obj,
        //Binary,
        //Bool,
        //Logical
        Count
    }

}
