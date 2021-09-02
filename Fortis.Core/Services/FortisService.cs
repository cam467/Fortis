namespace Fortis.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fortis.Core.Entities;
    using Fortis.Core.Models;
    using Fortis.Core.Repositories;
    using System.Data;

    public class FortisService : IFortisService
    {
        private readonly ISettings _settings;
        private readonly ILogs _logs;
        private readonly IFortisApi _api;
        private readonly IADExtensions _ad;
        private readonly IKronosApi _kronos;
        private readonly IGlobal _global;
        private readonly IRazorService _razorservice;
        private readonly IUserRepository _userrepository;
        private readonly IGroupRepository _grouprepository;

        public FortisService(ISettings settings, ILogs logs, IFortisApi api, IADExtensions ad, IKronosApi kronos, IGlobal global, IRazorService razorservice, IUserRepository userrepository, IGroupRepository grouprepository)
        {
            this._settings = settings;
            this._logs = logs;
            this._api = api;
            this._ad = ad;
            this._kronos = kronos;
            this._global = global;
            this._razorservice = razorservice;
            this._grouprepository = grouprepository;
            this._userrepository = userrepository;
        }

        public string TestConnection()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(_api.GetUsers());
        }

        public List<User> GetAllADAccounts()
        {
            List<User> users = new List<User>();
            DataTable adset = _settings.GetTableFromJson(_settings.GetSettingValue("admonitorgroups"));
            List<ADGroup> adgroups = new List<ADGroup>();
            if (adset == null) return users;
            var columns = adset.Columns.Cast<DataColumn>().Select((x,i) => new {name=x.ColumnName,index=i});
            int coldomain = columns.Any(x => x.name.ToLower().Contains("domain")) ? columns.First(x => x.name.ToLower().Contains("domain")).index : -1,
                colgroup = columns.Any(x => x.name.ToLower().Contains("group")) ? columns.First(x => x.name.ToLower().Contains("group")).index : -1,
                colroot = columns.Any(x => x.name.ToLower().Contains("root")) ? columns.First(x => x.name.ToLower().Contains("root")).index : -1;
            if (coldomain == -1) return users;
            foreach(DataRow r in adset.Rows)
            {
                try
                {
                    adgroups.Add(new ADGroup {domain = r[coldomain].ToString(), adgroup = r[colgroup].ToString().Split("|").ToList(), ouroot = r[colroot].ToString()});
                }
                catch (Exception ex)
                {
                    _logs.NewLog(ex,"GetAllADAccounts error: " + ex.Message + ex.StackTrace);
                    continue;
                }
            }
            
            foreach (ADGroup group in adgroups)
            {
                users.AddRange(_ad.GetAllADUsersForGroups(group.adgroup,group.ouroot));
            }
            return users;
        }

        public bool SyncADUsersWithKBUsers()
        {
            var adusers = GetAllADAccounts();
            var active = adusers.Select(x => new { name = x.first_name + " " + x.last_name, email = x.email, status = x.status });
            _logs.NewLog("ad users: " + Newtonsoft.Json.JsonConvert.SerializeObject(active));
            var kbusers = _userrepository.GetUsers();
            var ademails = adusers.Where(x => x.status == "True").Select(x => x.email);
            var kbemails = kbusers.Select(x => x.email);
            var addusers = adusers.Where(x => !kbemails.Contains(x.email));
            _logs.NewLog("add users: " + Newtonsoft.Json.JsonConvert.SerializeObject(addusers));
            return true;
        }

        public bool RunExport()
        {
            string emailto = _settings.GetSettingValue("importlogemailto");
            var users = GetAllADAccounts();
            //need to go through and get all the actives only and then delete the inactives
            List<User> dbusers = _api.GetUsers();
            // _logs.NewLog("dbusers: " + Newtonsoft.Json.JsonConvert.SerializeObject(dbusers));
            var dbemails = dbusers.Where(x => x.status == "active" && x.custom_field_1 == "adsynced").Select(x => x.email.ToLower()).ToList();
            //only get the active users
            var ademails = users.Where(x => x.status.ToLower() == "true").Select(x => x.email.ToLower()).ToList();
            //Do comparison and filter out users already in db with same email address
            List<User> newusers = users.Where(x => !dbemails.Contains(x.email.ToLower()) && x.status.ToLower() == "true").ToList();
            List<User> deleteusers = dbusers.Where(x => !ademails.Contains(x.email.ToLower()) && x.status == "active").ToList();
            var newemails = newusers.Select(x => x.email.ToLower()).ToList();
            var delemails = deleteusers.Select(x => x.email.ToLower()).ToList();
            List<User> updateusers = dbusers.Where(x => !delemails.Contains(x.email.ToLower()) && !newemails.Contains(x.email.ToLower()) && x.status == "active").ToList();
            // _logs.NewLog("updateusers: count " + updateusers.Count.ToString() + " - " + Newtonsoft.Json.JsonConvert.SerializeObject(updateusers));
            //Get Users employee ids from kronos
            KronosFoundResults x = this._kronos.GetUsers(newusers);
            KronosFoundResults y = this._kronos.GetUsers(updateusers);
            newusers = x.success;
            updateusers = y.success;
            List<User> failed = x.failed;
            failed.AddRange(y.failed);

            // _logs.NewLog(failed.Count.ToString() + " users with no Kronos link");
            // _logs.NewLog("update users with no Kronos link: " + Newtonsoft.Json.JsonConvert.SerializeObject(failed));
            // _logs.NewLog("update users: " + Newtonsoft.Json.JsonConvert.SerializeObject(updateuserss));
            // _logs.NewLog("new users: " + Newtonsoft.Json.JsonConvert.SerializeObject(newuserss));
            // //Update Fortis with AD user data
            _logs.NewLog("adding " + newusers.Count.ToString() + " new users...");
            _api.AddUsers(newusers);
            _logs.NewLog("deleting " + deleteusers.Count.ToString() + " users...");
            // _logs.NewLog("deleting users: " + Newtonsoft.Json.JsonConvert.SerializeObject(deleteusers));
            _api.ArchiveUsers(deleteusers);
            _logs.NewLog("updating " + updateusers.Count.ToString() + " users...");
            // _logs.NewLog("updating users: " + Newtonsoft.Json.JsonConvert.SerializeObject(updateusers));
            _api.UpdateUsers(updateusers);
            //After upload, no need to add users to the db as they will import on next import automatically
            //Need to add an import report and send out via email
            ReportStatsView reportmodel = new ReportStatsView
            {
                addedcount = newusers.Count,
                updatecount = updateusers.Count,
                deletedcount = deleteusers.Count,
                addedusers = newusers,
                deletedusers = deleteusers,
                nokronosusers = failed,
                urllink = _global.urllink
            };
            // string emailbody = "";

            // _logs.NewLog("reportmodel: " + Newtonsoft.Json.JsonConvert.SerializeObject(reportmodel));
            // try
            // {
            // string path = AppDomain.CurrentDomain.BaseDirectory;
            // string filepath = $"{path}Views\\Email\\importlogemail.cshtml";
            string emailbody = _razorservice.RunCompile("importlogemail.cshtmls", reportmodel);
            // }
            // catch (Exception ex)
            // {
            // 	_logs.NewLog("Import Report send error: " + ex.Message);
            // }
            _global.SendEmail(emailto, "Fortis Sync Log", emailbody);
            return true;
        }

        public bool RunImport()
        {
            // AddGroups();
            // AddUsers();
            // AddStorePurchases();
            // AddEnrollments();
            // AddCampaigns();
            // AddAccount();
            return true;
        }

        public List<User> GetUsers()
        {
            // var users = this._repo.GetUsers();
            var users = this._api.GetUsers();
            _logs.NewLog("users: " + Newtonsoft.Json.JsonConvert.SerializeObject(users));
            // _logs.NewLog("users count: " + users.Count.ToString());
            return new List<User>();
        }

        public List<KGroup> GetGroups()
        {
            var groups = this._grouprepository.GetGroups();
            // _logs.NewLog("groups: " + Newtonsoft.Json.JsonConvert.SerializeObject(groups));
            return groups;
        }

        public bool AddUsers()
        {
            // AddGroups();
            var users = _api.GetUsers();
            //only add users if they have a first and lastname
            users = users.Where(x => !String.IsNullOrWhiteSpace(x.first_name) && !String.IsNullOrWhiteSpace(x.last_name)).ToList();
            return _userrepository.AddUsers(users);
        }
        
        public bool AddGroups()
        {
            var groups = _api.GetGroups();
            // _logs.NewLog("groups:" + Newtonsoft.Json.JsonConvert.SerializeObject(groups));
            return _grouprepository.AddGroups(groups);
        }
        // public bool AddStorePurchases()
        // {
        //     var purchases = _api.GetStorePurchases();
        //     return _repo.AddStorePurchases(purchases);
        // }
        // public bool AddEnrollments()
        // {
        //     var enrollments = _api.GetEnrollments();
        //     return _repo.AddEnrollments(enrollments);
        // }
        // public bool AddCampaigns()
        // {
        //     var campaigns = _api.GetCampaigns();
        //     return _repo.AddCampaigns(campaigns);
        // }
        // public bool AddAccount()
        // {
        //     var account = _api.GetAccount();
        //     return _repo.AddAccount(account);
        // }
    }
}