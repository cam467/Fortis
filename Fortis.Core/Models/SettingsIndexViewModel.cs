namespace Fortis.Core.Models
{
    using System.Data;
    using System.Collections.Generic;
    public class SettingsIndexViewModel
    {
        public string pagetitle {get;set;}
        public string sectionidselected { get; set; }
        public string menucolor { get; set; }
        public DataTable menus { get; set; }
        public List<Setting> settings { get; set; }
        public int configview { get; set; }
    }
}