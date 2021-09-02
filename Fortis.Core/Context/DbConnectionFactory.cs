namespace Fortis.Core.Contexts
{
    using System;
    using System.Data;
    using System.Data.SQLite;
    using System.Data.SqlClient;
    using System.Text.RegularExpressions;

    public class DbConnectionFactory : IDbConnectionFactory
    {
        private string connectionstring { get; set; }
        private string dbtype { get; set; }
        public string pass { get; set; }
        public string user { get; set; }
        public string domain { get; set; }
        public bool trusted { get; set; }

        public DbConnectionFactory(string connectionstring, string type)
        {
            this.connectionstring = connectionstring;
            this.dbtype = type;

            //if the connection string contains Trusted_Connection and username and password info, strip it and use it for impersonation
            this.trusted = false;
            if (connectionstring.ToLower().IndexOf("trusted_connection=true") > 0)
            {
                this.trusted = true;
                if (Regex.IsMatch(connectionstring,"User Id=(.*?);",RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline))
                {
                    this.user = Regex.Match(connectionstring, "User Id=(.*?);", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline).Groups[1].Value;
                    this.domain = this.user.IndexOf('\\') != -1 ? this.user.Split('\\')[0] : "";
                    this.user = this.user.IndexOf('\\') != -1 ? this.user.Split('\\')[1] : user;
                    this.pass = Regex.Match(connectionstring, "Password=(.*?);", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline).Groups[1].Value;
                }
            }
        }

        public IDbConnection CreateDbConnection()
        {
            return CreateDbConnection(this.connectionstring, this.dbtype);
        }

        private IDbConnection CreateDbConnection(string connectionstring, string type)
        {
            //TODO: Need to implement Impersonation for database access in this 
            if (string.IsNullOrWhiteSpace(connectionstring)) throw new ArgumentNullException("Connection string is not specified");

            switch (type.ToLower())
            {
                case "sqlite":
                    return new SQLiteConnection(connectionstring);
                case "sqlserver":
                    return new SqlConnection(connectionstring);
                default:
                    throw new ArgumentNullException("Connection type is not specified");
            }
        }
    }
}