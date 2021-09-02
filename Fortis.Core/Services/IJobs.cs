namespace Fortis.Core.Services
{
    using System.Collections.Generic;
    using Fortis.Core.Models;
    public interface IJobs
    {
        List<Job> GetJobs();
    }
}