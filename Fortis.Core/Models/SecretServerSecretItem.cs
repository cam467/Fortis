namespace Fortis.Core.Models
{
    public class SecretServerSecretItem
    {
        public string fieldDescription { get; set; }
        public int fieldId { get; set; }
        public string fieldName { get; set; }
        public int? fileAttachmentId { get; set; }
        public string filename { get; set; }
        public bool isFile { get; set; }
        public bool isNotes { get; set; }
        public bool isPassword { get; set; }
        public int itemId { get; set; }
        public string itemValue { get; set; }
        public string slug { get; set; }
    }
}