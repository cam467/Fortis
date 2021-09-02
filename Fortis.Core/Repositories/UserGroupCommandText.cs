namespace Fortis.Core.Repositories
{
    using System;

    public class UserGroupCommandText : IUserGroupCommandText
    {
        public string InsertUserGroups => "insert into user_groups (user_id,group_id) values (@user_id,@group_id);";
    }
}