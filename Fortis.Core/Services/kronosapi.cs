namespace Fortis.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using Dapper;
    using Fortis.Core.Models;
    using Fortis.Core.Entities;

    public class KronosApi : IKronosApi
    {
        private readonly ISettings _settings;
        private readonly ILogs _log;
        private readonly IImpersonate _impersonate;
        private readonly IKronosCommandText _commands;

        public KronosApi(ISettings settings, ILogs log, IImpersonate impersonate, IKronosCommandText commandtext)
        {
            this._settings = settings;
            this._log = log;
            this._commands = commandtext;
            this._impersonate = impersonate;
            SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.DateTime2);
        }

        private SqlConnection ConnectDatabase()
        {
            string a = _settings.GetSettingValue("dbkronosserver"),
                b = _settings.GetSettingValue("dbkronosdefaultdatabase"),
                c = _settings.GetSettingValue("dbkronostrusted"),
                d = _settings.GetSettingValue("dbkronosusername"),
                h = _settings.GetSettingValue("dbkronospassword"),
                f = d.IndexOf('\\') != -1 ? d.Split('\\')[0] : "",
                g = d.IndexOf('\\') != -1 ? d.Split('\\')[1] : d,
                e = "server=" + a + ";database=" + b + ";" + (c == "0" && !String.IsNullOrWhiteSpace(g) ? "User ID=" + g + ";Password=" + h + ";" : c == "1" ? "Trusted_Connection=True;" : "");

            SqlConnection db = new SqlConnection(e);

            if (c == "1" && !String.IsNullOrWhiteSpace(g))
            {
                try
                {
                    //open the connection as the impersonated user
                    _impersonate.RunAsImpersonated(() =>
                    {
                        db.Open();
                        return true;
                    }, g, h, f);
                }
                catch (Exception ex)
                {
                    _log.NewLog("ConnectDatabase error opening connection \"" + a + "\": " + ex.Message);
                }
            }

            // db = new SqlConnection(e);
            return db;
        }

        public KronosFoundResults GetUsers(List<User> users)
        {
            string sql0 = this._commands.SelectUserById,
                sql1 = this._commands.SelectUserByFirstLastName,
                sql2 = this._commands.SelectUserByLastNameEmail;

            KronosFoundResults results = new KronosFoundResults();
            results.failed = new List<User>();
            results.success = new List<User>();

            using (SqlConnection cn = ConnectDatabase())
            {
                foreach (User user in users)
                {
                    User u;
                    try
                    {
                        if (!String.IsNullOrWhiteSpace(user.employee_number))
                        {
                            u = cn.QueryFirst<User>(sql0, new { employeenumber = user.employee_number });
                        }
                        else
                        {
                            u = cn.QueryFirst<User>(sql1, new { lastname = "%" + user.last_name + "%", firstname = "%" + user.first_name + "%" });
                        }
                    }
                    catch
                    {
                        try
                        {
                            u = cn.QueryFirst<User>(sql2, new { lastname = "%" + user.last_name + "%", email = "%" + user.email + "%" });
                        }
                        catch (Exception ex)
                        {

                            results.success.Add(new User
                            {
                                id = user.user_id,
                                employee_number = user.employee_number,
                                first_name = user.first_name,
                                last_name = user.last_name,
                                email = user.email,
                                manager_email = user.manager_email,
                                manager_name = user.manager_name
                            });
                            results.failed.Add(new User
                            {
                                id = user.user_id,
                                employee_number = user.employee_number,
                                first_name = user.first_name,
                                last_name = user.last_name,
                                email = user.email,
                                manager_email = user.manager_email,
                                manager_name = user.manager_name
                            });
                            // _log.NewLog("User " + user.first_name + " " + user.last_name + " " + user.email + " is not being added");
                            u = new User();
                        }
                    }
                    if (!String.IsNullOrWhiteSpace(u.employee_number))
                    {
                        // user.employee_number = u.employee_number.Trim();
                        // user.manager_email = String.IsNullOrWhiteSpace(u.manager_email) ? "" : u.manager_email.Trim();
                        // user.manager_name = String.IsNullOrWhiteSpace(u.manager_name) ? "" : u.manager_name.Trim();
                        results.success.Add(new User
                        {
                            id = user.user_id,
                            employee_number = u.employee_number.Trim(),
                            first_name = user.first_name,
                            last_name = user.last_name,
                            email = user.email,
                            manager_email = String.IsNullOrWhiteSpace(u.manager_email) ? "" : u.manager_email.Trim(),
                            manager_name = String.IsNullOrWhiteSpace(u.manager_name) ? "" : u.manager_name.Trim()
                        });
                    }
                }
                return results;
            }
        }
    }
}