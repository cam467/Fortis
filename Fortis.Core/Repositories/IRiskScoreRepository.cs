namespace Fortis.Core.Repositories
{
    using System.Collections.Generic;
    using Fortis.Core.Entities;

    public interface IRiskScoreRepository
    {
        bool AddRiskScore(int id, RiskScore score);
        List<RiskScore> GetByID(int id);
    }
}