namespace Fortis.Core.Services
{
    using System.Data;
    using System.Collections.Generic;
    using Fortis.Core.Models;
    
    public interface ISettings
    {
        bool DeleteSetting(string _key);
        List<Setting> GetEditSetting(string _key);
        bool UpdateSetting(List<Setting> setting);
        bool CreateSetting(List<Setting> setting);
        bool SaveSettingValue(string _key,string _value);
        string GetSettingValue(string _key);
        DataTable GetTableFromJson(string _json);
        string GetJsonFromTable(DataTable _table);
        List<Setting> GetSettings(int _section);
        Setting GetSetting(string _key);
        bool SaveSettings(List<Setting> _settings);
    }
}