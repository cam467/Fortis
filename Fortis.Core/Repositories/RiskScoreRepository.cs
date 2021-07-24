namespace KnowBe4.Core.Repositories
{
    using System;
    using System.Collections.Generic;
    using KnowBe4.Core.Entities;
    using KnowBe4.Core.Contexts;
    using KnowBe4.Core.Services;

    public class RiskScoreRepository : BaseRepository, IRiskScoreRepository
    {
        private readonly IRiskScoreCommandText _queries;
        private readonly ILogs _logs;
        public RiskScoreRepository(IDbConnectionFactory connectionfactory, IRiskScoreCommandText queries, ILogs logs, IImpersonate impersonate) : base(connectionfactory, logs, impersonate)
        {
            this._queries = queries;
            this._logs = logs;
        }

        public List<RiskScore> GetByID(int id)
        {
            return Query<RiskScore>(this._queries.GetRiskScoresByUser, new { id = id });
        }

        public bool AddRiskScore(int id, RiskScore score)
        {
            try
            {
                Execute(this._queries.InsertRiskScores, new { id = id, score_type = score.score_type, score_id = score.score_id, risk_score = score.risk_score, score_date = score.date });
            }
            catch (Exception ex)
            {
                _logs.NewLog("(AddGroups) RiskScore " + score.score_type.ToString() + " - " + ex.Message + ex.StackTrace);
            }
            return true;
        }
    }
}