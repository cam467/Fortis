namespace KnowBe4.Web.Controllers
{
    using System.Collections.Generic;
    using KnowBe4.Core.Entities;
    using KnowBe4.Core.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ILogs _logs;
        private readonly IKnowBe4Service _service;

        public ApiController(ILogs logs,IKnowBe4Service service)
        {
            this._service = service;
            this._logs = logs;
        }

        [HttpGet("testsettings")]
        public ActionResult<string> TestSettings()
        {
            return this._service.TestConnection();
        }

        [HttpGet("users")]
        public ActionResult<List<User>> GetUsers()
        {
            List<User> users = this._service.GetUsers();
            return users;
        }

        [HttpGet("groups")]
        public ActionResult<List<KGroup>> GetGroups()
        {
            List<KGroup> groups = this._service.GetGroups();
            return groups;
        }

        [HttpGet("addusers")]
        public ActionResult<bool> AddUsers()
        {
            bool r = this._service.AddUsers();
            return r;
        }

        [HttpGet("syncusers")]
        public ActionResult<bool> SyncUsers()
        {
            bool r = this._service.SyncADUsersWithKBUsers();
            return r;
        }

        [HttpGet("addgroups")]
        public ActionResult<bool> AddGroups()
        {
            bool r = this._service.AddGroups();
            return r;
        }

        [HttpGet("adusers")]
        public ActionResult<List<User>> GetADUsers()
        {
            List<User> users = this._service.GetAllADAccounts();
            _logs.NewLog("adusers count: " + users.Count.ToString());
            return users;
        }

        [HttpGet("import")]
        public ActionResult<bool> RunImport()
        {
            return this._service.RunImport();
        }

        [HttpGet("export")]
        public ActionResult<bool> RunExport()
        {
            return this._service.RunExport();
        }
    }
}