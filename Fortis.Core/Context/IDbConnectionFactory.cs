namespace Fortis.Core.Contexts
{
    using System.Data;

    public interface IDbConnectionFactory
    {
        public string pass { get; set; }
        public string user { get; set; }
        public string domain { get; set; }
        public bool trusted { get; set; }
        IDbConnection CreateDbConnection();
    }

}