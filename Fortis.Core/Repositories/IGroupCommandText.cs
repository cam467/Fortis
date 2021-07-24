namespace KnowBe4.Core.Repositories
{
    public interface IGroupCommandText
    {
        string GetGroups { get; }
        string GetGroupByID { get; }
        string GetGroupByName { get; }
        string InsertGroups { get; }
        string UpdateGroups { get; }
    }
}