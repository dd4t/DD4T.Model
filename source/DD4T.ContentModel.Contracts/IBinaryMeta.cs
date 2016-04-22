using System;

namespace DD4T.ContentModel
{
    public interface IBinaryMeta
    {
        string Id { get; }
        string VariantId { get; }
        DateTime LastPublishedDate { get; }
        bool HasLastPublishedDate { get; }
    }
}
