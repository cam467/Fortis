namespace Fortis.Core.Models
{
    public class Job
    {
        public string jobid {get;set;}
        public string jobname {get;set;}
        public string jobcron {get;set;}
        public string jobclass {get;set;}
        public string jobparam {get;set;}
        public bool jobactive {get;set;}
    }
}