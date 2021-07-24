namespace KnowBe4.Core.Repositories
{
    using System.Collections.Generic;
    using KnowBe4.Core.Entities;

    public interface IRiskScoreRepository
    {
        bool AddRiskScore(int id, RiskScore score);
        List<RiskScore> GetByID(int id);
    }
}