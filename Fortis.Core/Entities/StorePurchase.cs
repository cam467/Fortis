namespace Fortis.Core.Entities
{
    using System;
        
    public class StorePurchase
        {
            public long store_purchase_id { get; set; }
            public string content_type { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string type { get; set; }
            public int duration { get; set; }
            public bool retired { get; set; }
            public DateTime retirement_date { get; set; }
            public DateTime publish_date { get; set; }
            public string publisher { get; set; }
            public DateTime purchase_date { get; set; }
            public string policy_url { get; set; }
	}
}