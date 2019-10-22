using System.Collections.Generic;
namespace DD4T.ContentModel
{
    public interface IRegion : IViewable
    {
        string Name { get; }
        ISchema Schema { get; }
        IList<IComponentPresentation> ComponentPresentations { get; }
        IList<IRegion> Regions { get; }
        IFieldSet MetadataFields { get; }
    }
}