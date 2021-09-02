namespace Fortis.Core.Services
{
    using System;
    public interface ILogs
    {
        bool ClearLog();
        void NewLog(string _description);
        void NewLog(Exception e, string _description);
        bool IsDebug();
    }
}