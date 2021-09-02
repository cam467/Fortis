namespace Fortis.Core.Migrations
{
    using FluentMigrator;

    [Migration(20210402021412)]
    [Tags("settings")]
    public class IntializeSettingsDB : Migration
    {
        public override void Up()
        {
            Create.Table("Logs")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("machinename").AsString().NotNullable()
                .WithColumn("logged").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("level").AsString().NotNullable()
                .WithColumn("message").AsString().NotNullable()
                .WithColumn("logger").AsString().Nullable()
                .WithColumn("callsite").AsString().Nullable()
                .WithColumn("exception").AsString().Nullable();
            
            Create.Table("Settings")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()               
                .WithColumn("key").AsString().NotNullable()
                .WithColumn("name").AsString().NotNullable()
                .WithColumn("value").AsString().NotNullable()
                .WithColumn("section").AsInt32().NotNullable()
                .WithColumn("type").AsString().NotNullable()
                .WithColumn("active").AsBoolean().NotNullable().WithDefaultValue(1)
                .WithColumn("order").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("values").AsString().Nullable();
            
            //Seed table
            Insert.IntoTable("Settings")
                .Row(new {id = 1, key = "key", name = "Control Key", value = "", section = "-1", type = "text", active = true, order = 1})
                .Row(new {id = 2, key = "name", name = "Control Name", value = "", section = "-1", type = "text", active = true, order = 2})
                .Row(new {id = 3, key = "value", name = "Control Default Value", value = "", section = "-1", type = "text", active = true, order = 3})
                .Row(new {id = 4, key = "section", name = "Menu Section ID", value = "", section = "-1", type = "text", active = true, order = 4})
                .Row(new {id = 5, key = "type", name = "Control Type", value = "", section = "-1", type = "select", active = true, order = 5,values = "text,textsearch,password,button,html,select,checkbox,radio,table,columnlist,tableedit"})
                .Row(new {id = 6, key = "order", name = "Control Order", value = "", section = "-1", type = "text", active = true, order = 6})
                .Row(new {id = 7, key = "values", name = "Control Values (for select,radio,checkbox,columnlist,table...comma delimited)", value = "", section = "-1", type = "text", active = true, order = 7})
                .Row(new {id = 8, key = "cfgmenusections", name = "Menu Sections", value = "[{\"Section_Name\":\"Database Configuration\",\"Section_ID\":\"2\",\"Section_Title\":\"Database Configuration\",\"Section_Icon\":\"fa-database\"},{\"Section_Name\":\"Schedules\",\"Section_ID\":\"3\",\"Section_Title\":\"Schedules\",\"Section_Icon\":\"fa-clock-o\"},{\"Section_Name\":\"Logs\",\"Section_ID\":\"4\",\"Section_Title\":\"Logs\",\"Section_Icon\":\"fa-book\"},{\"Section_Name\":\"Site Configuration\",\"Section_ID\":\"0\",\"Section_Title\":\"Site Configuration\",\"Section_Icon\":\"fa-wrench\"}]", section = "0", type = "tableedit", active = true, order = 3})
                .Row(new {id = 9, key = "cfgsitetitle", name = "Site Title", value = "Default Site Title", section = "0", type = "text", active = true, order = 1})
                .Row(new {id = 10, key = "cfgheadertitle", name = "Header Title", value = "Default Header Title", section = "0", type = "text", active = true, order = 2})
                .Row(new {id = 11, key = "cfgmenucolor", name = "Menu Color", value = "blue", section = "0", type = "radio", active = true, order = 5, values = "green,blue,red,yellow"})
                .Row(new {id = 12, key = "cfgheadercolor", name = "Header Color", value = "blue", section = "0", type = "radio", active = true, order = 4, values = "green,blue,red,yellow"})
                .Row(new {id = 13, key = "cfgheadericon", name = "Header Icon", value = "fa-icon", section = "0", type = "text", active = true, order = 6})
                .Row(new {id = 14, key = "logviewer", name = "Logs", value = "", section = "4", type = "table", active = true, order = 1,values = "query:select id, logged date, level event, message || ' | ' || exception description from Logs order by logged desc;"})
                .Row(new {id = 15, key = "logtofile", name = "Output Log to File", value = "0", section = "4", type = "checkbox", active = true, order = 3})
                .Row(new {id = 16, key = "logdebug", name = "Debug", value = "0", section = "4", type = "checkbox", active = true, order = 4})
                .Row(new {id = 17, key = "logclearlog", name = "Purge Logs", value = "Purge", section = "4", type = "button", active = true, order = 2})
                .Row(new {id = 18, key = "logemailalertsto", name = "Email Alerts To", value = "", section = "4", type = "text", active = true, order = 20})
                .Row(new {id = 19, key = "schactive", name = "Pause Schedule", value = "No", section = "3", type = "radio", active = true, order = 2,values = "Yes,No"})
                .Row(new {id = 20, key = "schschedules", name = "Schedules", value = "[{\"Schedule_Name\":\"Daily at 5 am\",\"Cron_Expression\":\"0 0 5 1/1 * ? *\",\"Job_Class\":\"Schedules.Jobs.UploadAllTemplatesJob\",\"Job_Param\":\"\",\"Active\":\"false\"}]", section = "3", type = "tableedit", active = true, order = 1})
                .Row(new {id = 21, key = "schrestart", name = "Restart", value = "Restart", section = "3", type = "button", active = true, order = 3})
                .Row(new {id = 22, key = "dbserver", name = "Database Server", value = "", section = "2", type = "text", active = true, order = 1})
                .Row(new {id = 23, key = "dbdefaultdatabase", name = "Default Database", value = "", section = "2", type = "text", active = true, order = 2})
                .Row(new {id = 24, key = "dbusername", name = "User Name", value = "", section = "2", type = "text", active = true, order = 3})
                .Row(new {id = 25, key = "dbpassword", name = "Password", value = "", section = "2", type = "password", active = true, order = 4})
                .Row(new {id = 26, key = "dbtrusted", name = "Trusted Connection", value = "1", section = "2", type = "checkbox", active = true, order = 4});
                
        }

        public override void Down()
        {
            Delete.Table("Settings");
            Delete.Table("Logs");
        }
    }
}