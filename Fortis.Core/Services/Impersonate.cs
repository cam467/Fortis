namespace Fortis.Core.Services
{
    using System;
    using System.Security.Principal;
    using System.Runtime.InteropServices;
    using Microsoft.Win32.SafeHandles;
    
    public class Impersonate : IImpersonate
    {
        public const int LOGON32_PROVIDER_DEFAULT = 0;
        public const int LOGON32_LOGON_INTERACTIVE = 2;
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
        int dwLogonType, int dwLogonProvider, out SafeAccessTokenHandle phToken);

        private readonly ILogs _logs;

        public Impersonate(ILogs logs)
        {
            _logs = logs;
        }

        private SafeAccessTokenHandle GetAccessToken(string username, string password, string domain)
        {
            SafeAccessTokenHandle safeAccessTokenHandle;
            bool returnValue = LogonUser(username, domain, password,  
                LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,  
                out safeAccessTokenHandle);

            if (false == returnValue)  
            {  
                int ret = Marshal.GetLastWin32Error();  
                _logs.NewLog(String.Format("LogonUser failed with error code : {0}", ret));  
                throw new System.ComponentModel.Win32Exception(ret);  
            }

            return safeAccessTokenHandle;
        }

        public T RunAsImpersonated<T>(Func<T> func, string username, string password, string domain)
        {
            try
            {
                if (_logs.IsDebug()) _logs.NewLog("About to Impersonate");
                SafeAccessTokenHandle safeAccessTokenHandle = GetAccessToken(username,password,domain);
                if (_logs.IsDebug()) _logs.NewLog("under Impersonate");
                return WindowsIdentity.RunImpersonated(safeAccessTokenHandle, () => {
                    if (_logs.IsDebug()) _logs.NewLog("Delegate is running under account:" + WindowsIdentity.GetCurrent().Name);
                    return func();
                });
            }
            catch(Exception e)
            {
                _logs.NewLog(e,"RunAsImpersonated error: " + e.Message);
                return default;
            }
        }
    }
}