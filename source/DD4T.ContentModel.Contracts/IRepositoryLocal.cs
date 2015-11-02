
namespace DD4T.ContentModel.Contracts
{ 
    public interface IRepositoryLocal : IItem
    {
        IPublication Publication { get; }
        IPublication OwningPublication { get; }
    }
}
