namespace KnowBe4.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using KnowBe4.Core.Entities;
    using KnowBe4.Core.Contexts;
    using KnowBe4.Core.Services;

    public class UserGroupRepository : BaseRepository, IUserGroupRepository
    {
        private readonly IUserGroupCommandText _queries;
        private readonly ILogs _logs;
        public UserGroupRepository(IDbConnectionFactory connectionfactory, IUserGroupCommandText queries, ILogs logs, IImpersonate impersonate) : base(connectionfactory,logs,impersonate)
        {
            this._queries = queries;
            this._logs = logs;
        }

        public bool AddUserGroup(int user_id, int group_id)
        {
            try
            {
                Execute(this._queries.InsertUserGroups, new { user_id = user_id, group_id = group_id });
            }
            catch (Exception ex)
            {
                _logs.NewLog("(AddUserGroup) UserGroup " + user_id.ToString() + " - " + ex.Message + ex.StackTrace);
            }
            return true;
        }
    }
}