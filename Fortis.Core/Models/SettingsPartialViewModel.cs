namespace Fortis.Core.Models
{
    using System.Collections.Generic;
    public class SettingsPartialViewModel
    {
        public string pagetitle { get; set; }
        public List<Setting> settings { get; set; }
        public int configview { get; set; }
    }
}