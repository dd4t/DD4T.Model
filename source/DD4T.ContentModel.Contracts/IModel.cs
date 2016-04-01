using System.Collections.Generic;

namespace DD4T.ContentModel.Contracts
{
    public interface IModel
    {
        IDictionary<string, IFieldSet> ExtensionData { get; }
    }
}
