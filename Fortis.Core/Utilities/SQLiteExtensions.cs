namespace KnowBe4.Core.Utilities
{
    using System;
    using System.Data;
    using System.Data.SQLite;
    public static class SqliteExtensions
    {
        public static DataTable GetDataTable(this SQLiteConnection connection, string sql)
        {
            DataTable dt = new DataTable();
            connection.Open();
            SQLiteCommand mycommand = new SQLiteCommand(connection);
            mycommand.CommandText = sql;
            SQLiteDataReader reader = mycommand.ExecuteReader();
            dt.Load(reader);
            reader.Close();
            connection.Close();
            return dt;
        }
    }
}