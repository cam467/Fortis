namespace Fortis.Core.Models
{
    using System.Collections.Generic;
    public class SecretServerSecretSummary
    {
        public bool active { get; set; }
        public bool autoChangeEnabled { get; set; }
        public bool checkedOut { get; set; }
        public bool checkOutEnabled { get; set; }
        public string createDate { get; set; }
        public int daysUntilExpiration { get; set; }
        public List<SecretServerSecretSummaryExtendedField> extendedFields { get; set; }
        public int folderId { get; set; }
        public bool hidePassword { get; set; }
        public int Id { get; set; }
        public bool inheritsPermissions { get; set; }
        public bool isRestricted { get; set; }
        public string lastAccessed { get; set; }
        public string lastPasswordChangeAttempt { get; set; }
        public string name { get; set; }
        public string[] responseCodes { get; set; }
        public int secretTemplateId { get; set; }
        public string secretTemplateName { get; set; }
        public int siteId { get; set; }
    }
}