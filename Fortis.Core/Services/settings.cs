namespace Fortis.Core.Services
{
    using System;
    using System.Data;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Linq;
    using System.Reflection;
    using System.Data.SQLite;
    using Microsoft.Extensions.Configuration;
    using Dapper;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Fortis.Core.Models;
    using Fortis.Core.Utilities;

    public class Settings : ISettings
    {
        private readonly ILogs _log;
        private readonly IGlobal _global;
        private readonly IConfiguration _config;
        private readonly ISettingsCommandText _commands;
        private readonly ICrypto _crypto;
        private readonly string _hpass = "ak3#r9391!D";
        public Settings(ILogs log, IGlobal global, IConfiguration config, ISettingsCommandText commands, ICrypto crypto)
        {
            _log = log;
            _global = global;
            _config = config;
            _commands = commands;
            _crypto = crypto;
        }

        public bool DeleteSetting(string _key)
        {
            using (SQLiteConnection cn = new SQLiteConnection(_global.sqlitedb))
            {
                try
                {
                    cn.Execute(_commands.DeleteSetting,new {key = _key});
                    return true;
                }
                catch (System.Exception e)
                {
                    _log.NewLog(e,"DeleteSetting error: " + e.Message);
                    return false;
                }
            }
        }

        public List<Setting> GetEditSetting(string _key)
        {
            List<Setting> sets = GetSettings(-1);
            Setting set = GetSetting(_key);
            foreach (Setting s in sets)
            {
                // s.value = set[s.name];
                switch (s.id)
                {
                    case "glokey":
                        s.value = set.id;
                        break;
                    case "gloname":
                        s.value = set.name;
                        break;
                    case "glotype":
                        s.value = set.type;
                        break;
                    case "glovalue":
                        s.value = set.value;
                        break;
                    case "glovalues":
                        s.value = set.values;
                        break;
                    case "glosection":
                        s.value = set.section.ToString();
                        break;
                    case "gloorder":
                        s.value = set.order.ToString();
                        break;
                }
            }
            return sets;
        }

        public bool UpdateSetting(List<Setting> setting)
        {
            using (SQLiteConnection cn = new SQLiteConnection(_global.sqlitedb))
            {
                Setting n = new Setting {
                    id = HttpUtility.UrlDecode(setting.First(x => x.name.ToLower().Contains("glokey")).value),
                    name = HttpUtility.UrlDecode(setting.First(x => x.name.ToLower().Contains("gloname")).value),
                    value = HttpUtility.UrlDecode(setting.First(x => x.name.ToLower().Contains("glovalue")).value),
                    section = int.Parse(HttpUtility.UrlDecode(setting.First(x => x.name.ToLower().Contains("glosection")).value)),
                    type = HttpUtility.UrlDecode(setting.First(x => x.name.ToLower().Contains("glotype")).value),
                    order = int.Parse(HttpUtility.UrlDecode(setting.First(x => x.name.ToLower().Contains("gloorder")).value)),
                    values = HttpUtility.UrlDecode(setting.First(x => x.name.ToLower().Contains("glovalues")).value),
                    previousid = setting.First().previousid
                };
                if (n.type == "password") n.value = _crypto.Encrypt(n.value,_hpass);
                try
                {
                    //fix the order first then add
                    cn.Execute(_commands.UpdateSetting,new {key = n.id,name = n.name,value = n.value,section = n.section,type = n.type,order = n.order,values = n.values,previouskey = n.previousid});
                    return true;
                }
                catch (System.Exception e)
                {
                    _log.NewLog(e,"UpdateSetting error: " + e.Message);
                    return false;
                }
            }
        }

        public bool CreateSetting(List<Setting> setting)
        {
            using (SQLiteConnection cn = new SQLiteConnection(_global.sqlitedb))
            {
                Setting n = new Setting {
                    id = HttpUtility.UrlDecode(setting.First(x => x.name.ToLower().Contains("glokey")).value),
                    name = HttpUtility.UrlDecode(setting.First(x => x.name.ToLower().Contains("gloname")).value),
                    value = HttpUtility.UrlDecode(setting.First(x => x.name.ToLower().Contains("glovalue")).value),
                    section = int.Parse(HttpUtility.UrlDecode(setting.First(x => x.name.ToLower().Contains("glosection")).value)),
                    type = HttpUtility.UrlDecode(setting.First(x => x.name.ToLower().Contains("glotype")).value),
                    order = int.Parse(HttpUtility.UrlDecode(setting.First(x => x.name.ToLower().Contains("gloorder")).value)),
                    values = HttpUtility.UrlDecode(setting.First(x => x.name.ToLower().Contains("glovalues")).value)
                };
                if (n.type == "password") n.value = _crypto.Encrypt(n.value,_hpass);
                try
                {
                    //fix the order first then add
                    cn.Execute(_commands.CreateSetting,new {key = n.id,name = n.name,value = n.value,section = n.section,type = n.type,order = n.order,values = n.values});
                    return true;
                }
                catch (System.Exception e)
                {
                    _log.NewLog(e,"CreateSetting error: " + e.Message);
                    return false;
                }
            }
        }

        public DataTable GetTableFromJson(string _json)
        {
            var json = JToken.Parse(_json);
            DataTable table = new DataTable();
            if (json.Type!=JTokenType.Array) return table;
            var rows = json.Children();
            var cols = rows.First();

            //collect the columns and add
            foreach (var col in cols)
            {
                var prop = col.Value<JProperty>();
                table.Columns.Add(prop.Name, typeof(String));
            }
            
            //collect the row data
            foreach (var row in rows)
            {
                var drow = table.NewRow();
                var tokens = row.Children();
                foreach (var token in tokens)
                {
                    var prop = token.Value<JProperty>();
                    drow[prop.Name] = prop.Value;
                }
                table.Rows.Add(drow);
            }
            return table;
        }

        public string GetJsonFromTable(DataTable _table)
        {
            return JsonConvert.SerializeObject(_table);
        }

        public string GetSettingValue(string _key)
        {
            using (SQLiteConnection cn = new SQLiteConnection(_global.sqlitedb))
            {
                var r = cn.Query<Setting>(_commands.GetSettingValue,new {key = _key}).DefaultIfEmpty(new Setting{id="",name="",type="",value="",table=null}).First();
                if (r.name.ToLower().Contains("password"))
                {
                    return _crypto.Decrypt(r.value, _hpass);
                }
                return r.value;
            }
        }

        public bool SaveSettingValue(string _key, string _value)
        {
            using (SQLiteConnection cn = new SQLiteConnection(_global.sqlitedb))
            {
                try
                {
                    var r = cn.Execute(_commands.SaveSettingValue,new {key = _key,value = _value});
                    return true;
                }
                catch (Exception ex)
                {
                    _log.NewLog(ex,"SaveSettingValue error: " + ex.Message);
                    return false;
                }
            }
        }

        private List<Setting> ProcessSettings(List<Setting> settings)
        {
            foreach (Setting r in settings)
            {
                switch (r.type)
                {
                    case "columnlist":
                    case "table":
                        Match m = Regex.Match(r.values??"",@"^(.*):");
                        if (m.Success && m.Groups.Count>0)
                        {
                            switch (m.Groups[1].Value)
                            {
                                case "query":
                                    using (SQLiteConnection conn = new SQLiteConnection(_config.GetConnectionString("settings")))
                                    {
                                        r.table = conn.GetDataTable(r.values.Split(':')[1]);
                                    }
                                    break;
                                case "function":
                                    /*This code block uses Reflection to dynamically load a class and call a method in that
                                    class based on a string. This string must have a format of: [namespace].[class]([constructor parameters]*note: no parenthesis for default constructor).[method]([method parameters or blank for none]*note: parenthesis are required). Optionally you can provide a name of a globalparam to map comma delimited values to the object by adding :mapping:valueofglobalparam to the end of above string. The method must return a 
                                    list of an object in order to be used. This list of object will then be converted to a 
                                    datatable to be used by the template and displayed.
                                    */
                                    Regex rx = new Regex(@"function:([\w\.]+)(?:(\(.*?\)))?\.(.*?)\((.*?)\)");
                                    var md = rx.Match(r.values);
                                    bool inst = md.Groups[2].Value.Contains("(");
                                    var cparms = Regex.Replace(md.Groups[2].Value,"[()]","").Split(',');
                                    var parms = md.Groups[4].Value.Split(',');
                                    //prepare method parameters if any. Returns empty string array if none
                                    for (int i=0;i<parms.Length;i++)
                                    {
                                        parms[i] = parms[i].Contains("password") ? _crypto.Decrypt(this.GetSettingValue(parms[i]),_hpass) : this.GetSettingValue(parms[i]);
                                    }
                                    //prepare constructor parameters if any. Return empty string array if none
                                    for (int i=0;i<cparms.Length;i++)
                                    {
                                        cparms[i] = cparms[i].Contains("password") ? _crypto.Decrypt(this.GetSettingValue(cparms[i]),_hpass) : this.GetSettingValue(cparms[i]);
                                    }
                                    //cast parameters to object array
                                    object[] parama = (object[])parms;
                                    object[] cparama = (object[])cparms;
                                    //check if the class is static or needs an object instance to call the method
                                    if (inst) 
                                    {
                                        Type ty = Type.GetType(md.Groups[1].Value);
                                        MethodInfo dispose = ty.GetMethod("Dispose");
                                        MethodInfo method = ty.GetMethod(md.Groups[3].Value);
                                        //this creates the object instance
                                        var _class = Activator.CreateInstance(ty,cparama);
                                        var _obj = method.Invoke(_class, parama);
                                        //this takes the object and converts it to a datatable
                                        r.table = _global.ConvertObjectToTable(_obj);
                                        if (dispose!=null) dispose.Invoke(_class,null);
                                    }
                                    else
                                    {
                                        //this code block is for static classes
                                        Type ty = Type.GetType(md.Groups[1].Value);
                                        MethodInfo mi = ty.GetMethod(md.Groups[3].Value);
                                        var _obj = mi.Invoke(ty,parama);
                                        r.table = _global.ConvertObjectToTable(_obj);
                                    }
                                    break;
                            }
                            // //Now check if value is empty and if not then begin mapping to first column
                            if (!string.IsNullOrWhiteSpace(r.value) && r.table!=null && r.table.Rows.Count>0)
                            {
                                string[] vals = null;
                                if (r.type == "columnlist")
                                {
                                    vals = r.value.Split(new string[] {"%3B"}, StringSplitOptions.None);
                                }
                                else
                                {
                                    vals = r.value.Split(',');
                                }
                                DataTable dt = r.table;
                                if (vals.Length>0)
                                {
                                    int iter = dt.Rows.Count > vals.Length ? vals.Length : dt.Rows.Count;
                                    for (int i=0;i<iter;i++)
                                    {
                                        dt.Rows[i][0] = vals[i];
                                    }
                                    r.table = dt;
                                }
                            }
                        }
                        break;
                    case "tableedit":
                        if (!String.IsNullOrWhiteSpace(r.value))
                        {
                            r.table = GetTableFromJson(r.value);
                        }
                        break;
                }
            }
            return settings;
        }

        public List<Setting> GetSettings(int _section)
        {
            using (SQLiteConnection cn = new SQLiteConnection(_global.sqlitedb))
            {
                List<Setting> res = cn.Query<Setting>(_commands.GetSettings,new {section = _section}).ToList();
                if (res!=null)
                {
                    res = ProcessSettings(res);
                }
                return res;
            }
        }

        public Setting GetSetting(string _key)
        {
            using (SQLiteConnection cn = new SQLiteConnection(_global.sqlitedb))
            {
                List<Setting> res = cn.Query<Setting>(_commands.GetSetting,new {key = _key}).ToList();
                if (res!=null)
                {
                    res = ProcessSettings(res);
                }
                return res.First();
            }
        }

        public bool SaveSettings(List<Setting> _settings)
        {
            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(_global.sqlitedb))
                {
                    foreach(Setting s in _settings)
                    {
                        if (s.name.ToLower().Contains("password")) 
                        {
                            if (!s.value.Contains("password")) cn.Execute(_commands.SaveSettings,new {value = _crypto.Encrypt(HttpUtility.UrlDecode(s.value),_hpass),key = s.name});
                        }
                        else
                        {
                            cn.Execute(_commands.SaveSettings,new {value = HttpUtility.UrlDecode(s.value),key = s.name});
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _log.NewLog(ex,ex.Message);
                return false;
            }
            return true;
        }
    }
}