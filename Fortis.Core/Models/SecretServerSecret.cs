namespace KnowBe4.Core.Models
{
    using System.Collections.Generic;
    public class SecretServerSecret
    {
        public bool active { get; set; }
        public bool autoChangeEnabled { get; set; }
        public string autoChangeNextPassword { get; set; }
        public int id { get; set; }
        public int folderId { get; set; }
        public bool isOutOfSync { get; set; }
        public bool isRestricted { get; set; }
        public string lastHeartBeatCheck { get; set; }
        public string lastPasswordChangeAttempt { get; set; }
        public string name { get; set; }
        public List<SecretServerSecretItem> items { get; set; }
        public bool requiresApprovalForAccess { get; set; }
        public string[] responseCodes { get; set; }
        public int secretTemplateId { get; set; }
        public string secretTemplateName { get; set; }
    }
}