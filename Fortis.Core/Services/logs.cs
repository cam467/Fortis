namespace KnowBe4.Core.Services
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Data.SQLite;

    public class Logs : ILogs
    {
        private readonly ILogger<Logs> _logger;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceprovider;
        private ISettings _settings;
        private bool debug {get;set;}

        public Logs(ILogger<Logs> logger, IConfiguration config, IServiceProvider serviceprovider)
        {
            this._logger = logger;
            this._config = config;
            this._serviceprovider = serviceprovider;
        }

        public bool IsDebug()
        {
            if (_settings == null)
            {
                _settings = _serviceprovider.GetService<ISettings>();
                debug = _settings.GetSettingValue("logdebug") == "1" ? true : false;
            }
            return debug;
        }
        
        public bool ClearLog()
        {
            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(_config.GetConnectionString("settings")))
                {
                    cn.Open();
                    string sql = "delete from log;";
                    SQLiteCommand cmd = new SQLiteCommand(sql,cn);
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.NewLog(ex,"ClearLog error: " + ex.Message);
            }
            return true;
        }

        public void NewLog(string _description)
        {
            _logger.LogInformation(_description);
        }

        public void NewLog(Exception e, string _description)
        {
            _logger.LogError(e,_description);
        }
    }
}