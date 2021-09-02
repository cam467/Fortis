namespace Fortis.Core.Repositories
{
    using System;

    public class RiskScoreCommandText : IRiskScoreCommandText
    {
        public string InsertRiskScores => "insert into riskscores(id,score_type,score_id,risk_score,score_date) values (@id,@score_type,@score_id,@risk_score,@score_date);";

        public string GetRiskScoresByUser => "select * from riskscores where id = @id";
    }
}