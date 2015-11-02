using System.Collections.Generic;

namespace DD4T.ContentModel.Contracts
{
    public interface IComponentPresentation : IModel
    {
        IComponent Component { get; }
        IComponentTemplate ComponentTemplate { get; }
        IPage Page { get; set; }
        bool IsDynamic { get; set; }
        string RenderedContent { get; }
        int OrderOnPage { get; set; }
        IList<ICondition> Conditions { get; } 
    }
}
