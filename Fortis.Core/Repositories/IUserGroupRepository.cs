namespace KnowBe4.Core.Repositories
{
    public interface IUserGroupRepository
    {
        bool AddUserGroup(int user_id, int group_id);
    }
}