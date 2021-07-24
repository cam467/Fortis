namespace KnowBe4.Core.Services
{
    public interface IKronosCommandText
    {
        string SelectUserById { get; }
        string SelectUserByFirstLastName { get; }
        string SelectUserByLastNameEmail { get; }
    }
}