using System.Collections.Generic;

namespace DD4T.ContentModel.Contracts
{
    public interface ITargetGroup
    {
        string Description { get; set; }
        IList<ICondition> Conditions { get; }
    }
}