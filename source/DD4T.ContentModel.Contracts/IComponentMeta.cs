
namespace DD4T.ContentModel.Contracts
{
    using System;
    public interface IComponentMeta
    {
        DateTime ModificationDate { get; }
        DateTime CreationDate { get; }
        DateTime LastPublishedDate { get; }
    }
}
