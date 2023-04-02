using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

using IfcDb.Exceptions;
using IfcDb.Interfaces;
using IfcDb.Models;

namespace IfcDb.Parsers
{
    public class IfcParser : IIfcParser
    {

        private IfcAttributeType getType(string str)
        {
            string[] patterns = {
                @"(?<null>^\$$)",
                @"(?<der>^\*$)",
                @"(?<int>^(-|\+)?\d+$)",
                @"(?<real>^(-|\+)?\d*(\.\d*)?(E)?(-|\+)?\d*$)",
                @"(?<str>^\'.*\')",
                @"(?<ent>^#\d+$)",
                @"(?<enum>^\..+\.$)",
                @"(?<list>^\((.+,)*.*\)$)",
                @"(?<obj>^(#\d+\s?=\s?)?\w+\(.+\);?$)"
            };
            var pattern = string.Join("|", patterns);
            var reg = new Regex(pattern);
            string[] gNames = { "null", "der", "int", "real", "str", "ent", "enum", "list", "obj" };
            var m = reg.Match(str);
            foreach (string gName in gNames)
            {
                if (m.Groups[gName].Success)
                {
                    switch (gName)
                    {
                        case "null": return IfcAttributeType.Null;
                        case "der": return IfcAttributeType.Derive;
                        case "int": return IfcAttributeType.Integer;
                        case "real": return IfcAttributeType.Real;
                        case "str": return IfcAttributeType.String;
                        case "ent": return IfcAttributeType.EntityInstanceName;
                        case "enum": return IfcAttributeType.Enumeration;
                        case "list": return IfcAttributeType.List;
                        case "obj": return IfcAttributeType.Obj;
                    }
                }
            }
            throw new IfcGetStringTypeFailedException(str);
        }

        private List<IfcAttribute> parseAttributeList(string str)
        {
            var result = new List<IfcAttribute>();
            if (str.Length == 0)
            {
                return result;
            }

            var i = 0;
            bool isStr = false;
            int start = i;
            int level = 0;

            while (i <= str.Length)
            {
                if (!isStr && level == 0 && (str.Length == i || str[i] == ','))
                {
                    result.Add(ParseAttribute(str.Substring(start, i - start)));
                    start = ++i;
                    continue;
                }

                if (str[i] == '\'')
                {
                    isStr = !isStr;
                }
                if (!isStr)
                {
                    if (str[i] == '(')
                    {
                        ++level;
                    }
                    if (str[i] == ')')
                    {
                        --level;
                    }
                }
                ++i;
            }

            if (isStr || level > 0)
            {
                throw new IfcObjAttributeListParsingFailedException(str);
            }
            return result;
        }

        private List<IfcObj> parseData(string pattern, string ifcString)
        {
            var reg = new Regex(pattern);
            var array = Regex.Split(reg.Match(ifcString).Groups["data"].Value, ";\r\n");
            var result = new List<IfcObj>();

            foreach (var str in array)
            {
                if (!string.IsNullOrEmpty(str) & (str != "\r\n"))
                {
                    result.Add(ParseObj(str));
                }
            }
            return result;
        }

        public IfcObj ParseObj(string str)
        {
            var pattern = @"#(?<id>\d+)\s?=\s?(?<ifcobj>\w+)\((?<attr>,?.+)\);?";
            var reg = new Regex(pattern);
            Match m = null;
            IfcObj res = null;
            if (reg.IsMatch(str))
            {
                m = reg.Match(str);
                res = new IfcObj
                {
                    Id = int.Parse(m.Groups["id"].Value),
                    Name = m.Groups["ifcobj"].Value,
                    Attributes = parseAttributeList(m.Groups["attr"].Value)
                };
                return res;
            }

            pattern = @"(?<ifcobj>\w+)\((?<attr>,?.+)\);?";
            var reg2 = new Regex(pattern);
            if (!reg2.IsMatch(str))
            {
                throw new IfcObjParsingFailedException(str);
            }
            m = reg2.Match(str);
            res = new IfcObj
            {
                Id = null,
                Name = m.Groups["ifcobj"].Value,
                Attributes = parseAttributeList(m.Groups["attr"].Value)
            };
            return res;
        }

        public IfcAttribute ParseAttribute(string str)
        {
            IfcAttribute result = new IfcAttribute();
            var value = str.Trim();
            result.Type = getType(value);
            result.Value = string.Empty;
            switch (result.Type)
            {
                case IfcAttributeType.Null:
                    result.Value = string.Empty;
                    break;
                case IfcAttributeType.Derive:
                    result.Value = string.Empty;
                    break;
                case IfcAttributeType.String:
                    result.Value = value;
                    break;
                case IfcAttributeType.Real:
                    result.Value = double.Parse(value, CultureInfo.InvariantCulture);
                    break;
                case IfcAttributeType.Integer:
                    result.Value = int.Parse(value);
                    break;
                case IfcAttributeType.EntityInstanceName:
                    result.Value = int.Parse(value.Substring(1));
                    break;
                case IfcAttributeType.Enumeration: //!!!
                    result.Value = value.Substring(1, value.Length - 2);
                    break;
                case IfcAttributeType.List:
                    result.Value = parseAttributeList(value.Substring(1, value.Length - 2));
                    break;
                case IfcAttributeType.Obj:
                    var obj = ParseObj(value);
                    obj.Destination = IfcObjDestination.Data;
                    result.Value = obj; 
                    break;
            }
            return result;
        }

        public IfcFile ParseFile(string str)
        {
            IfcFile file = new IfcFile();

            var headpattern = @"HEADER;\r\n(?<data>(.*\r\n)*?)ENDSEC;";
            var datapattern = @"DATA;\r\n(?<data>(.*\r\n)*?)ENDSEC;";

            file.Head = parseData(headpattern, str);
            file.Data = parseData(datapattern, str);

            return file;
        }
    }
}
