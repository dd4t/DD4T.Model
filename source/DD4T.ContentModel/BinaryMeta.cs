using System;

namespace DD4T.ContentModel
{
    public class BinaryMeta : IBinaryMeta
    {
        public string VariantId { get; set; }
        public string Id { get; set; }
        public DateTime LastPublishedDate { get; set; }
        public bool HasLastPublishedDate { get; set; }
    }
}
