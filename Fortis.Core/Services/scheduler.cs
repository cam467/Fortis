namespace KnowBe4.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Quartz;
    using Quartz.Spi;
    using KnowBe4.Core.Models;

    public class Jobs : IJobs
    {
        private ISettings _settings;
        private ILogs _log;
        public Jobs(ISettings settings,ILogs log)
        {
            _settings = settings;
            _log = log;
        }

        public List<Job> GetJobs()
        {
            DataTable jobset = _settings.GetTableFromJson(_settings.GetSettingValue("schschedules"));
            List<Job> jobs = new List<Job>();
            if (jobset == null) return jobs;
            var columns = jobset.Columns.Cast<DataColumn>().Select((x,i) => new {name=x.ColumnName,index=i});
            int colname = columns.Any(x => x.name.ToLower().Contains("name")) ? columns.First(x => x.name.ToLower().Contains("name")).index : -1,
                colcron = columns.Any(x => x.name.ToLower().Contains("cron")) ? columns.First(x => x.name.ToLower().Contains("cron")).index : -1,
                colactive = columns.Any(x => x.name.ToLower().Contains("active")) ? columns.First(x => x.name.ToLower().Contains("active")).index : -1,
                colparam = columns.Any(x => x.name.ToLower().Contains("param")) ? columns.First(x => x.name.ToLower().Contains("param")).index : -1,
                colclass = columns.Any(x => x.name.ToLower().Contains("class")) ? columns.First(x => x.name.ToLower().Contains("class")).index : -1;
            if (colname == -1) return jobs;
            foreach(DataRow r in jobset.Rows)
            {
                try
                {
                    jobs.Add(new Job {jobname = r[colname].ToString(), jobcron = r[colcron].ToString(), jobclass = r[colclass].ToString(), jobparam = r[colparam].ToString(), jobactive = bool.Parse(r[colactive].ToString())});
                }
                catch (Exception ex)
                {
                    _log.NewLog(ex,"GetJobs error: " + ex.Message);
                    continue;
                }
            }
            return jobs;
        }
    }

    public class SingletonJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public SingletonJobFactory(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return (IJob) this._serviceProvider.GetRequiredService(bundle.JobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        {

        } 
    }

    public class SchedulerHostedService : IHostedService, ISchedulers
    {
        private readonly ISettings _settings;
        private readonly ILogs _log;
        private readonly IJobs _jobs;
        private readonly ISchedulerFactory _schedulerfactory;
        private readonly IJobFactory _jobfactory;

        public IScheduler Scheduler {get;set;}

    	public SchedulerHostedService(ISettings settings, ILogs log, IJobs jobs, ISchedulerFactory schedulerFactory, IJobFactory jobFactory)
        {
            this._log = log;
            this._settings = settings;
            this._jobs = jobs;
            this._schedulerfactory = schedulerFactory;
            this._jobfactory = jobFactory;
            _log.NewLog("Trying to Start Scheduler");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerfactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobfactory;
            
            List<Job> jobs = _jobs.GetJobs();
            foreach (Job j in jobs)
            {
                try
                {
                    if (!j.jobactive) continue;
                    IJobDetail job = JobBuilder.Create(Type.GetType(j.jobclass,true)).WithIdentity(j.jobname).Build();
                    job.JobDataMap["param"] = j.jobparam;
                    ITrigger trig = TriggerBuilder.Create().WithCronSchedule(j.jobcron).Build();
                    await Scheduler.ScheduleJob(job,trig,cancellationToken);
                    _log.NewLog("Job "+j.jobname+" Scheduled");
                }
                catch (Exception ex)
                {
                    _log.NewLog(ex,"Schedule Jobs error: " + ex.Message);
                    continue;
                }
            }
            await Scheduler.Start();
            _log.NewLog("Scheduler Started");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Scheduler.Shutdown(cancellationToken);
                _log.NewLog("Scheduler Shutdown");
            }
            catch (Exception ex)
            {
                _log.NewLog(ex,"Scheduler Shutdown error:" + ex.Message);
            }
        }

        public bool Resume()
        {
            try
            {
                if (InStandbyMode() && !Scheduler.IsShutdown) Scheduler.Start();
                if (Scheduler.IsShutdown)
                {
                    var result = StartAsync(new CancellationToken());
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool StandBy()
        {
            try
            {
                Scheduler.Standby();
                return true;
            }
            catch
            {
                return false;   
            }
        }

        public bool Stop()
        {
            try
            {
                if (!Scheduler.IsShutdown) Scheduler.Shutdown();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsStarted()
        {
            bool b;
            try
            {
                b = Scheduler.IsStarted && !Scheduler.IsShutdown;
            }
            catch
            {
                b = false;
            }
            return b;
        }

        public bool InStandbyMode()
        {
            bool b;
            try
            {
                b = Scheduler.InStandbyMode;
            }
            catch
            {
                b = false;
            }
            return b;
        }

        public bool TriggerJob(string jobname)
        {
        	try
        	{
        		Scheduler.TriggerJob(new JobKey(jobname));
        		return true;
        	}
        	catch
        	{
        		return false;
        	}
        }
    }
}