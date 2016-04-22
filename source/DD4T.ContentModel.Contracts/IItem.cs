namespace DD4T.ContentModel
{
    public interface IItem : IModel
    {
        string Id { get; }
        string Title { get; }
    }
}
