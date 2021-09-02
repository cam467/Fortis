namespace Fortis.Core.Services
{
    using System;
    
    public interface ISettingsCommandText
    {
        string DeleteSetting {get;}
        string UpdateSetting {get;}
        string CreateSetting {get;}
        string GetSettingValue {get;}
        string SaveSettingValue {get;}
        string GetSettings {get;}
        string GetSetting {get;}
        string SaveSettings {get;}
    }
}