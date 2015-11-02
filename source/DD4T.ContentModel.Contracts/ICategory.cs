using System.Collections.Generic;

namespace DD4T.ContentModel.Contracts
{
    public interface ICategory : IRepositoryLocal
    {
        IList<IKeyword> Keywords { get; }
    }
}
