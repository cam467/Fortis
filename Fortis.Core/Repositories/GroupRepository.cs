namespace Fortis.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using Fortis.Core.Entities;
    using Fortis.Core.Contexts;
    using Fortis.Core.Services;

    public class GroupRepository : BaseRepository, IGroupRepository
    {
        private readonly IGroupCommandText _queries;
        private readonly ILogs _logs;
        private readonly IRiskScoreRepository _riskscorerepo;
        public GroupRepository(IDbConnectionFactory connectionfactory, IGroupCommandText queries, ILogs logs,IImpersonate impersonate,IRiskScoreRepository riskscorerepo) : base(connectionfactory,logs,impersonate)
        {
            this._queries = queries;
            this._logs = logs;
            this._riskscorerepo = riskscorerepo;
        }

        public KGroup GetByID(int group_id)
        {
            return QuerySingle<KGroup>(this._queries.GetGroupByID, new { group_id = group_id });
        }

        public KGroup GetByName(int name)
        {
            return QuerySingle<KGroup>(this._queries.GetGroupByName, new { name = name });
        }

        public List<KGroup> GetGroups()
        {
            return Query<KGroup>(this._queries.GetGroups);
        }

        public bool AddGroups(List<KGroup> groups)
        {
            foreach (var group in groups)
            {
                try
                {
                    Execute(this._queries.InsertGroups, group);

                    //insert risk score history
                    foreach (RiskScore score in group.risk_score_history)
                    {
                        score.score_id = "group";
                        score.score_type = "group";
                        this._riskscorerepo.AddRiskScore(group.group_id,score);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("insert duplicate key", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        try
                        {
                            Execute(this._queries.UpdateGroups, group);
                        }
                        catch (Exception e)
                        {
                            _logs.NewLog("(AddGroups) Group " + group.group_id.ToString() + " - " + e.Message + e.StackTrace);
                        }
                    }
                    else
                    {
                        _logs.NewLog("(AddGroups) Group " + group.id.ToString() + " - " + ex.Message + ex.StackTrace);
                    }
                }
            }
            return true;
        }
    }
}