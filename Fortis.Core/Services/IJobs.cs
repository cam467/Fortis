namespace KnowBe4.Core.Services
{
    using System.Collections.Generic;
    using KnowBe4.Core.Models;
    public interface IJobs
    {
        List<Job> GetJobs();
    }
}