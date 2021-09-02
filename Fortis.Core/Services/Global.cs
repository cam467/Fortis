namespace Fortis.Core.Services
{
	using System;
	using System.Net.Mail;
	using System.IO;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Data;
	using System.Configuration;
	using Microsoft.Extensions.Configuration;

	public class Global : IGlobal
	{
		public string hpass 
		{
			get {
				return "ak3#r9391!D";
			}
			set {}
		}
		public string sqlitedb 
		{
			get {
				return _config.GetConnectionString("settings");
			}
			set {}
		}
		public string workingdirectory
		{
			get {
				return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			}
			set {}
		}
		public string urllink 
		{
			get {
				return _config.GetValue<string>("urllink");
			}
			set {}
		}
		public string mailhost 
		{
			get {
				return _config.GetValue<string>("mailhost");
			}
			set {}
		}
		public string templatesource 
		{
			get {
				return Path.Combine(Assembly.GetEntryAssembly().Location,"Views");
			}
			set {}
		}

		private readonly ILogs _log;
		private readonly IConfiguration _config;

		public Global(ILogs log, IConfiguration config)
		{
			_log = log;	
			_config = config;
		}

		public string SendEmail(string to, string subject, string body)
		{
		    try
		    {
		        using (MailMessage mail = new MailMessage())
		       	{
			        mail.Body = body;
			        mail.IsBodyHtml = true;
			        foreach (var t in to.Split(new [] {";"}, StringSplitOptions.RemoveEmptyEntries))
					{
					    mail.To.Add(t);
					}
			        mail.From = new MailAddress("noreply@buc-ees.com", "Fortis (NoReply)", System.Text.Encoding.UTF8);
			        mail.Subject = subject;
			        mail.SubjectEncoding = System.Text.Encoding.UTF8;
			        mail.Priority = MailPriority.Normal;
			        using (SmtpClient smtp = new SmtpClient())
			        {
				        smtp.Host = mailhost;
				        smtp.Send(mail);
				    }
			    }
			    return "true";
		    }
		    catch (Exception ex)
		    {
		        _log.NewLog(ex,"Mail send error: " + ex.Source + "-" + ex.Message);
		        return ex.Source + "-" + ex.Message;
		    }
		}
		
		public string SendEmailAttachments(string to, string subject, string body, Dictionary<string,byte[]> attachments)
		{
		    try
		    {
		        using (MailMessage mail = new MailMessage())
		       	{
			        mail.Body = body;
			        mail.IsBodyHtml = true;
			        foreach (var t in to.Split(new [] {";"}, StringSplitOptions.RemoveEmptyEntries))
					{
					    mail.To.Add(t);
					}
			        mail.From = new MailAddress("noreply@buc-ees.com", "Fortis (NoReply)", System.Text.Encoding.UTF8);
					foreach(var attach in attachments)
					{
						mail.Attachments.Add(new Attachment(new MemoryStream(attach.Value),attach.Key));
					}
			        mail.Subject = subject;
			        mail.SubjectEncoding = System.Text.Encoding.UTF8;
			        mail.Priority = MailPriority.Normal;
			        using (SmtpClient smtp = new SmtpClient())
			        {
				        smtp.Host = mailhost;
				        smtp.Send(mail);
				    }
			    }
			    return "true";
		    }
		    catch (Exception ex)
		    {
		        _log.NewLog(ex,"Mail send error: " + ex.Source + "-" + ex.Message);
		        return ex.Source + "-" + ex.Message;
		    }
		}
		
		public Int64 GetTime()
		{
			Int64 retval=0;
			DateTime st=  new DateTime(1970,1,1);
			TimeSpan t= (DateTime.Now.ToUniversalTime()-st);
			retval= (Int64)(t.TotalMilliseconds+0.5);
			return retval;
		}

		public DataTable ConvertObjectToTable(object _class)
		{
			DataTable dt = new DataTable();
			PropertyInfo[] props = null;
			var coll = _class as IEnumerable<object>;
			foreach (var o in coll) {
				props = o.GetType().GetProperties();
				foreach (var prop in props) {
					dt.Columns.Add(prop.Name);
				}
				break;
			}
			foreach (var o in coll) {
				var r = dt.NewRow();
				foreach (var prop in props) {
					r[prop.Name] = prop.GetValue(o);
				}
				dt.Rows.Add(r);
			}
			return dt;
		}

		public static string GetUrlLink()
		{
			return ConfigurationManager.AppSettings["urllink"];
		}

		public static string GetHpass()
		{
			return "ak3#r9391!D";
		}

		public static string GetSqlLiteDb()
		{
			return "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\app_data\\appdata.s3db";
		}
	}
}