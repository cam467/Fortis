namespace KnowBe4.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using KnowBe4.Core.Services;
    using KnowBe4.Core.Models;
    using Microsoft.AspNetCore.Mvc.Filters;

    [Route("Settings")]
    public class SettingsController : Controller
    {
        private readonly ILogs _logs;
        private readonly ISettings _settings;
        private readonly ISchedulers _scheduler;
        private readonly IKnowBe4Api _api;

        public SettingsController(ILogs logs,ISettings settings,ISchedulers scheduler,IKnowBe4Api api)
        {
            _logs = logs;
            _settings = settings;
            _scheduler = scheduler;
            _api = api;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            ViewBag.headertitle = _settings.GetSettingValue("cfgheadertitle");
            ViewBag.headericon = _settings.GetSettingValue("cfgheadericon");
            ViewBag.headercolor = _settings.GetSettingValue("cfgheadercolor");
        }

        [HttpGet("{id?}")]
        public IActionResult Index(int id = 1)
        {
            string menucolor = _settings.GetSettingValue("cfgmenucolor");
            string sections = _settings.GetSettingValue("cfgmenusections");
            var menusections = _settings.GetTableFromJson(sections);
            var sets = _settings.GetSettings(id);
            return View(new SettingsIndexViewModel {pagetitle="",sectionidselected=id.ToString(), menucolor=menucolor, menus=menusections, settings=sets, configview=id });
        }

        [HttpPost("")]
        public bool SaveSettings([FromBody]List<Setting> sets)
        {
            return _settings.SaveSettings(sets);
        }

        [HttpDelete("{key}")]
        public async Task<bool> DeleteSetting(string key)
        {
            return await Task.FromResult<bool>(_settings.DeleteSetting(key));
        }

        [HttpPost("Partial/{id}")]
        public IActionResult Partial(int id, string title)
        {
            var sets = _settings.GetSettings(id);
            return View(new SettingsPartialViewModel {pagetitle=title,settings=sets, configview=id});
        }

        [HttpGet("AddSetting")]
        public IActionResult AddSetting()
        {
            var sets = _settings.GetSettings(-1);
            return View(new SettingsPartialViewModel { pagetitle="",settings=sets,configview=-1 });
        }

        [HttpPost("AddSetting")]
        public bool AddSetting([FromBody]List<Setting> sets)
        {
            return _settings.CreateSetting(sets);
        }

        [HttpGet("EditSetting/{key}")]
        public IActionResult EditSetting(string key)
        {
            var sets = _settings.GetEditSetting(key);
            return View("addsetting", new SettingsPartialViewModel { pagetitle="", settings=sets, configview=-1 });
        }

        [HttpPut("EditSetting")]
        public bool EditSetting([FromBody]List<Setting> sets)
        {
            return _settings.UpdateSetting(sets);
        }

        [HttpGet("Log/Clear")]
        public bool ClearLogs()
        {
            return _logs.ClearLog();
        }

        [HttpGet("scheduler/isrunning")]
        public bool IsSchedulerRunning()
        {
            return _scheduler.IsStarted() && !_scheduler.InStandbyMode();
        }

        [HttpGet("scheduler/pause")]
        public bool PauseScheduler()
        {
            return _scheduler.StandBy();
        }

        [HttpGet("scheduler/start")]
        public bool StartScheduler()
        {
            return _scheduler.Resume();
        }

        [HttpGet("scheduler/stop")]
        public bool StopScheduler()
        {
            return _scheduler.Stop();
        }

        [HttpGet("scheduler/restart")]
        public bool RestartScheduler()
        {
            return _scheduler.Stop() && _scheduler.Resume();
        }

        [HttpGet("scheduler/trigger/{jobname}")]
        public bool TriggerJobScheduler(string jobname)
        {
            return _scheduler.TriggerJob(jobname);
        }
    }
}