namespace Fortis.Core.Services
{
    public class SettingsCommandText : ISettingsCommandText
    {
        public string DeleteSetting => "delete from globalparams where glokey = @key;";
        public string UpdateSetting => "update globalparams set glokey = @key,gloname = @name,glovalue = @value,glosection = @section,glotype = @type,gloorder = @order,glovalues = @values where @glokey = @previouskey;";
        public string CreateSetting => "insert into globalparams (glokey,gloname,glovalue,glosection,glotype,gloorder,glovalues) values (@key,@name,@value,@section,@type,@order,@values);";
        public string GetSettingValue => "select glokey name, glovalue value from globalparams where glokey = @key";
        public string SaveSettingValue => "update globalparams set glovalue = @value where glokey = @key";
        public string GetSettings => "select glokey id, gloname name, glotype type, glovalue value, glovalues [values] from globalparams where gloactive = 1 and glosection = @section order by gloorder";
        public string GetSetting => "select glokey id, gloname name, glotype type, glovalue value, glosection section, glovalues [values], gloorder [order] from globalparams where glokey = @key;";
        public string SaveSettings => "update globalparams set glovalue = @value where glokey = @key";
    }
}