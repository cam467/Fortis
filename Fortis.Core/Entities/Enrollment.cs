namespace KnowBe4.Core.Entities
{
    using System;

    public class Enrollment
    {
        public int enrollment_id { get; set; }
        public string content_type { get; set; }
        public string module_name { get; set; }
        public User user { get; set; }
        public string campaign_name { get; set; }
        public DateTime enrollment_date { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? completion_date { get; set; }
        public string status { get; set; }
        public int time_spent { get; set; }
        public bool policy_acknowledged { get; set; }
    }
}