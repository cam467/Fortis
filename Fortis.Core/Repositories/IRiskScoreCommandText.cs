namespace Fortis.Core.Repositories
{
    public interface IRiskScoreCommandText
    {
        string InsertRiskScores { get; }
        string GetRiskScoresByUser { get; }
    }
}