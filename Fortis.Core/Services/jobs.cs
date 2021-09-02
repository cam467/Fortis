namespace Fortis.Core.Services
{
    using System;
    using Quartz;
    using System.Threading.Tasks;

    public class UploadAllTemplatesJob : IJob
    {
        private readonly ISettings _settings;
        private readonly ILogs _log;
        private readonly ISchedulers _scheduler;
        private readonly ICachingProvider _cache;
        public UploadAllTemplatesJob(ISettings settings,ILogs log,ISchedulers scheduler,ICachingProvider cache)
        {
            this._settings = settings;
            this._log = log;
            this._scheduler = scheduler;
            this._cache = cache;
        }

        public Task Execute(IJobExecutionContext context)
        {
            bool pause = _settings.GetSettingValue("schactive") == "Yes" ? true : false;
            if (pause)
            {
                _scheduler.StandBy();
                return Task.CompletedTask;
            }
            if (_cache.CacheKeyExist("_jobrunning_"))
            {
                _log.NewLog("Import Job in progress...");
                return Task.CompletedTask;
            }
            _log.NewLog("Import Job started...");
            _cache.AddCache("_jobrunning_","running");
            // _service.UploadAllTemplates();
            _cache.RemoveCache("_jobrunning_");
            _log.NewLog("Import Job completed");
            return Task.CompletedTask;
        }
    }

    public class ExportJob : IJob
    {
        private readonly ISettings _settings;
        private readonly ILogs _log;
        private readonly ISchedulers _scheduler;
        // private readonly IExporter _exporter;
        private readonly ICachingProvider _cache;
        public ExportJob(ISettings settings,ILogs log,ISchedulers scheduler,ICachingProvider cache)
        {
            this._settings = settings;
            this._log = log;
            this._scheduler = scheduler;
            // this._exporter = exporter;
            this._cache = cache;
        }

        public Task Execute(IJobExecutionContext context)
        {
            bool pause = _settings.GetSettingValue("schactive") == "Yes" ? true : false;
            if (pause)
            {
                _scheduler.StandBy();
                return Task.CompletedTask;
            }
            if (_cache.CacheKeyExist("_jobrunning_"))
            {
                _log.NewLog("Export Job in progress...");
                return Task.CompletedTask;
            }
            _log.NewLog("Export Job started...");
            _cache.AddCache("_jobrunning_","running");
            // _exporter.RunExport();
            _cache.RemoveCache("_jobrunning_");
            _log.NewLog("Export Job completed");
            return Task.CompletedTask;
        }
    }
}