using System.Collections.Generic;

namespace DD4T.ContentModel
{
    public interface IModel
    {
        IDictionary<string, IFieldSet> ExtensionData { get; }
    }
}
