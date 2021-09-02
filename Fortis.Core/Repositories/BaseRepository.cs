namespace Fortis.Core.Repositories
{
    using Fortis.Core.Contexts;
    using System;
    using System.Data;
    using Dapper;
    using System.Collections.Generic;
    using Fortis.Core.Services;

    public class BaseRepository
    {
        private readonly IDbConnectionFactory _connectionfactory;
        private readonly ILogs _logs;
        private readonly IImpersonate _impersonate;
        public BaseRepository(IDbConnectionFactory connectionfactory, ILogs logs, IImpersonate impersonate)
        {
            this._connectionfactory = connectionfactory;
            this._logs = logs;
            this._impersonate = impersonate;
        }

        public T QueryFirst<T>(string query, object parameters = null)
        {
            // try
            // {
                if (_connectionfactory.trusted && !String.IsNullOrWhiteSpace(_connectionfactory.user))
                {
                    return _impersonate.RunAsImpersonated(() => {
                        using (IDbConnection conn = _connectionfactory.CreateDbConnection())
                        {
                            return conn.QueryFirst<T>(query, parameters);
                        }
                    },_connectionfactory.user,_connectionfactory.pass,_connectionfactory.domain);
                }
                else
                {
                    using (IDbConnection conn = _connectionfactory.CreateDbConnection())
                    {
                        return conn.QueryFirst<T>(query, parameters);
                    }
                }
            // }
            // catch (Exception ex)
            // {
            //     //Handle the exception
            //     _logs.NewLog("QueryFirst error: " + ex.Message + ex.StackTrace);
            //     return default; //Or however you want to handle the return
            // }
        }

        public T QueryFirstOrDefault<T>(string query, object parameters = null)
        {
            // try
            // {
                if (_connectionfactory.trusted && !String.IsNullOrWhiteSpace(_connectionfactory.user))
                {
                    return _impersonate.RunAsImpersonated(() => {
                        using (IDbConnection conn = _connectionfactory.CreateDbConnection())
                        {
                            return conn.QueryFirstOrDefault<T>(query, parameters);
                        }
                    },_connectionfactory.user,_connectionfactory.pass,_connectionfactory.domain);
                }
                else
                {
                    using (IDbConnection conn = _connectionfactory.CreateDbConnection())
                    {
                        return conn.QueryFirstOrDefault<T>(query, parameters);
                    }
                }
            // }
            // catch (Exception ex)
            // {
            //     //Handle the exception
            //     _logs.NewLog("QueryFirstOrDefault error: " + ex.Message + ex.StackTrace);
            //     return default; //Or however you want to handle the return
            // }
        }

        public T QuerySingle<T>(string query, object parameters = null)
        {
            // try
            // {
                if (_connectionfactory.trusted && !String.IsNullOrWhiteSpace(_connectionfactory.user))
                {
                    return _impersonate.RunAsImpersonated(() => {
                        using (IDbConnection conn = _connectionfactory.CreateDbConnection())
                        {
                            return conn.QuerySingle<T>(query, parameters);
                        }
                    },_connectionfactory.user,_connectionfactory.pass,_connectionfactory.domain);
                }
                else
                {
                    using (IDbConnection conn = _connectionfactory.CreateDbConnection())
                    {
                        return conn.QuerySingle<T>(query, parameters);
                    }
                }
            // }
            // catch (Exception ex)
            // {
            //     //Handle the exception
            //     _logs.NewLog("QuerySingle error: " + ex.Message + ex.StackTrace);
            //     return default; //Or however you want to handle the return
            // }
        }

        public T QuerySingleOrDefault<T>(string query, object parameters = null)
        {
            // try
            // {
                if (_connectionfactory.trusted && !String.IsNullOrWhiteSpace(_connectionfactory.user))
                {
                    return _impersonate.RunAsImpersonated(() => {
                        using (IDbConnection conn = _connectionfactory.CreateDbConnection())
                        {
                            return conn.QuerySingleOrDefault<T>(query, parameters);
                        }
                    },_connectionfactory.user,_connectionfactory.pass,_connectionfactory.domain);
                }
                else
                {
                    using (IDbConnection conn = _connectionfactory.CreateDbConnection())
                    {
                        return conn.QuerySingleOrDefault<T>(query, parameters);
                    }
                }
            // }
            // catch (Exception ex)
            // {
            //     //Handle the exception
            //     _logs.NewLog("QuerySingleOrDefault error: " + ex.Message + ex.StackTrace);
            //     return default; //Or however you want to handle the return
            // }
        }

        public List<T> Query<T>(string query, object parameters = null)
        {
            // try
            // {
                if (_connectionfactory.trusted && !String.IsNullOrWhiteSpace(_connectionfactory.user))
                {
                    return _impersonate.RunAsImpersonated(() => {
                        using (IDbConnection conn = _connectionfactory.CreateDbConnection())
                        {
                            return conn.Query<T>(query, parameters).AsList<T>();
                        }
                    },_connectionfactory.user,_connectionfactory.pass,_connectionfactory.domain);
                }
                else
                {
                    using (IDbConnection conn = _connectionfactory.CreateDbConnection())
                    {
                        return conn.Query<T>(query, parameters).AsList<T>();
                    }
                }
            // }
            // catch (Exception ex)
            // {
            //     //Handle the exception
            //     _logs.NewLog("Query<T> error: " + ex.Message + ex.StackTrace);
            //     return default; //Or however you want to handle the return
            // }
        }

        public void Execute(string query, object parameters = null)
        {
            // try
            // {
                if (_connectionfactory.trusted && !String.IsNullOrWhiteSpace(_connectionfactory.user))
                {
                    _impersonate.RunAsImpersonated(() => {
                        using (IDbConnection conn = _connectionfactory.CreateDbConnection())
                        {
                            conn.Execute(query, parameters);
                            return true;
                        }
                    },_connectionfactory.user,_connectionfactory.pass,_connectionfactory.domain);
                }
                else
                {
                    using (IDbConnection conn = _connectionfactory.CreateDbConnection())
                    {
                        conn.Execute(query, parameters);
                    }
                }
            // }
            // catch (Exception ex)
            // {
            //     //Handle the exception
            //     _logs.NewLog("Execute error: " + ex.Message + ex.StackTrace);
            // }
        }
    }
}