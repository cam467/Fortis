namespace Fortis.Core.Services
{
    using System.Threading.Tasks;
    using System.Threading;
    
    public interface ISchedulers
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        bool IsStarted();
        bool InStandbyMode();
        bool Resume();
        bool Stop();
        bool TriggerJob(string jobname);
        bool StandBy();
    }
}