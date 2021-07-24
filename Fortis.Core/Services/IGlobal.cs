namespace KnowBe4.Core.Services
{
    using System;
    using System.Data;
    using System.Collections.Generic;

    public interface IGlobal
    {
        string hpass {get;set;}
        string sqlitedb {get;set;}
        string urllink {get;set;}
        string mailhost {get;set;}
        string templatesource {get;set;}
        string workingdirectory {get;set;}
        string SendEmail(string to, string subject, string body);
        string SendEmailAttachments(string to, string subject, string body, Dictionary<string,byte[]> attachments);
        Int64 GetTime();
        DataTable ConvertObjectToTable(object _class);
    }    
}