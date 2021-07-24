namespace KnowBe4.Core.Models
{
    using System.Collections.Generic;
    public class SecretServerPagingSecretSummary
    {
        public int batchCount { get; set; }
        public int currentPage { get; set; }
        public bool hasNext { get; set; }
        public bool hasPrev { get; set; }
        public int nextSkip { get; set; }
        public int pageCount { get; set; }
        public int prevSkip { get; set; }
        public List<SecretServerSecretSummary> records {get;set;}
        public int skip { get; set; }
        public List<SecretServerSort> sortBy { get; set; }
        public bool success { get; set; }
        public int take { get; set; }
        public int total { get; set; }
    }
}