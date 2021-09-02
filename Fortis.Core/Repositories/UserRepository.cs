namespace Fortis.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using Fortis.Core.Entities;
    using Fortis.Core.Contexts;
    using Fortis.Core.Services;

    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly IUserCommandText _queries;
        private readonly ILogs _logs;
        private readonly IUserGroupRepository _grouprepo;
        private readonly IRiskScoreRepository _riskrepo;
        public UserRepository(IDbConnectionFactory connectionfactory, IUserCommandText queries, ILogs logs, IImpersonate impersonate, IUserGroupRepository grouprepo, IRiskScoreRepository riskrepo) : base(connectionfactory,logs,impersonate)
        {
            this._queries = queries;
            this._logs = logs;
            this._grouprepo = grouprepo;
            this._riskrepo = riskrepo;
        }

        public User GetByID(int id)
        {
            return QuerySingle<User>(this._queries.GetUserByID, new { user_id = id });
        }

        public User GetByEmployeeNumber(int eid)
        {
            return QuerySingle<User>(this._queries.GetUserByEmployeeNumber, new { employee_number = eid });
        }

        public List<User> GetUsers()
        {
            return Query<User>(this._queries.GetUsers);
        }

        public bool AddUsers(List<User> users)
        {
            foreach (var user in users)
            {
                try
                {
                    Execute(this._queries.InsertUsers, user);

                    //add the group relationship
                    foreach (int g in user.groups)
                    {
                        this._grouprepo.AddUserGroup(user.user_id,g);
                    }

                    //insert risk scores
                    foreach (RiskScore score in user.risk_score_history)
                    {
                        score.score_id = "user";
                        score.score_type = "user";
                        this._riskrepo.AddRiskScore(user.user_id,score);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("insert duplicate key", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        try
                        {
                            Execute(this._queries.UpdateUsers, user);
                        }
                        catch (Exception e)
                        {
                            _logs.NewLog("(AddUsers) User " + user.user_id.ToString() + " - " + e.Message + e.StackTrace);
                        }
                    }
                    else
                    {
                        _logs.NewLog("(AddUsers) User " + user.user_id.ToString() + " - " + ex.Message + ex.StackTrace);
                    }
                }
            }
            return true;
        }
    }
}