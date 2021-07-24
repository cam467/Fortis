namespace KnowBe4.Core.Models
{
    using System.Data;
    using Newtonsoft.Json;
    
    public class Setting
    {
        public string id {get;set;}
        public string name {get;set;}
        public string type {get;set;}
        public string value {get;set;}
        public string values {get;set;}
        [JsonIgnore]
        public DataTable table {get;set;}
        public int section {get;set;}
        public int order {get;set;}
        public string previousid {get;set;}
    }
}