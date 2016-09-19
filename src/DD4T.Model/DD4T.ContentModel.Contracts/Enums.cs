//using System.Runtime.Serialization;

//namespace DD4T.ContentModel
//{
//    public enum ComponentType { Multimedia, Normal }
//    public enum FieldType { Text, MultiLineText, Xhtml, Keyword, Embedded, MultiMediaLink, ComponentLink, ExternalLink, Number, Date }
//    public enum NumericalConditionOperator { [EnumMember] UnknownByClient = -2147483648, [EnumMember] Equals = 0, [EnumMember] GreaterThan = 1, [EnumMember] LessThan = 2, [EnumMember] NotEqual = 3, }
//    public enum ConditionOperator { [EnumMember] UnknownByClient = -2147483648, [EnumMember] Equals = 0, [EnumMember] GreaterThan = 1, [EnumMember] LessThan = 2, [EnumMember] NotEqual = 3, [EnumMember] StringEquals = 4, [EnumMember] Contains = 5, [EnumMember] StartsWith = 6, [EnumMember] EndsWith = 7, }
//}

namespace DD4T.ContentModel
{
    public enum ComponentType { Multimedia, Normal }
    public enum FieldType { Text, MultiLineText, Xhtml, Keyword, Embedded, MultiMediaLink, ComponentLink, ExternalLink, Number, Date }
    public enum NumericalConditionOperator { UnknownByClient = -2147483648, Equals = 0, GreaterThan = 1, LessThan = 2, NotEqual = 3, }
    public enum ConditionOperator { UnknownByClient = -2147483648, Equals = 0, GreaterThan = 1, LessThan = 2, NotEqual = 3, StringEquals = 4, Contains = 5, StartsWith = 6, EndsWith = 7, }
}
