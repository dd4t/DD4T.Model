using System;

namespace DD4T.ContentModel.Contracts
{
    public interface IComponentMeta
    {
        DateTime ModificationDate { get; }
        DateTime CreationDate { get; }
        DateTime LastPublishedDate { get; }
    }
}
