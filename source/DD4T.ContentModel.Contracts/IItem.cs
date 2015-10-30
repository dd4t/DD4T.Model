
namespace DD4T.ContentModel.Contracts
{
    public interface IItem : IModel
    {
        string Id { get; }
        string Title { get; }
    }
}
