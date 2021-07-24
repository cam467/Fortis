namespace KnowBe4.Core.Services
{
    using ExcelDataReader;
    using System.IO;
    using System;
    using System.Text.RegularExpressions;
    using System.Reflection;
    using System.Data;
    using System.Collections.Generic;

    public class ExcelRepository : IExcelRepository
    {
        private readonly ILogs _logs;
        private readonly ISettings _settings;

        public ExcelRepository(ILogs logs, ISettings settings)
        {
            this._logs = logs;
            this._settings = settings;
        }

        public List<T> LoadExcelFile<T>(Stream data)
        {
            DataSet result;
            try
            {
                using (var reader = ExcelReaderFactory.CreateReader(data))
                {
                    result = reader.AsDataSet(new ExcelDataSetConfiguration() 
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                            // TransformValue = (reader, n, value) => 
                            // {
                            //     return value.ToString();
                            // }
                        } 
                    });
                }
            }
            catch (Exception)
            {
                try
                {
                    using (var reader = ExcelReaderFactory.CreateCsvReader(data))
                    {
                        result = reader.AsDataSet(new ExcelDataSetConfiguration() 
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                                // TransformValue = (reader, n, value) => 
                                // {
                                //     return value.ToString();
                                // }
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logs.NewLog(ex,"LoadExcelFile error: " + ex.Message);
                    return new List<T>();
                }
            }

            try
            {
                if (result.Tables.Count > 0)
                {
                    List<T> newresult = new List<T>();
                    foreach (DataRow row in result.Tables[0].Rows)
                    {
                        Type temp = typeof(T);
                        T obj = Activator.CreateInstance<T>();  

                        foreach (DataColumn column in row.Table.Columns)  
                        {  
                            foreach (PropertyInfo pro in temp.GetProperties())  
                            {  
                                if (pro.Name.ToLower() == Regex.Replace(column.ColumnName,"[#\\/\\s$_]","",RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline).ToLower())
                                {
                                    pro.SetValue(obj, Convert.ToString(row[column.ColumnName]), null);  
                                }
                                else
                                {
                                    continue;
                                }
                            }  
                        }
                        newresult.Add(obj);
                    }
                    return newresult;
                }
                return new List<T>();
            }
            catch (Exception ex)
            {
                _logs.NewLog(ex,"LoadExcelFile error: " + ex.Message);
                return new List<T>();
            }
        }
    }
}