namespace KnowBe4.Core.Repositories
{
    public interface IUserCommandText
    {
        string GetUsers { get; }
        string GetUserByID { get; }
        string GetUserByEmployeeNumber { get; }
        string InsertUsers { get; }
        string UpdateUsers {get;}
    }
}