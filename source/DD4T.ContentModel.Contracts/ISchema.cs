
namespace DD4T.ContentModel.Contracts
{
    public interface ISchema : IRepositoryLocal
    {
        IOrganizationalItem Folder { get; }
        string RootElementName { get; }
    }
}
