namespace KnowBe4.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using KnowBe4.Core.Models;
    using KnowBe4.Core.Utilities;
    using Newtonsoft.Json;

    public class SecretServerApiV1 : IPasswordRepository
    {
        private readonly ILogs _logs;
        private readonly ISettings _settings;
        private SecretServerSecret lastsecret {get;set;}
        private HttpClient client {get;set;}
        private DateTime tokenexpires {get;set;}
        public SecretServerApiV1(ILogs logs, ISettings settings)
        {
            this._logs = logs;
            this._settings = settings;
        }

        public string TestService()
        {
            _logs.NewLog("Secret username: " + GetSecretUsername(6717));
            _logs.NewLog("Secret password: " + GetSecretPassword(6717));
            return "true";
        }

        private bool Login()
        {
            if (this.client != null && this.tokenexpires > DateTime.Now) return true;

            string baseurl = _settings.GetSettingValue("secretserverbaseurl"),
                oauthurl = _settings.GetSettingValue("secretservertokenurl"),
                username = _settings.GetSettingValue("secretserverusername"),
                password = _settings.GetSettingValue("secretserverpassword");
            var logincreds = new SecretServerLoginCreds {
                username = username,
                password = password
            };
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(baseurl);
            try
            {
                var logincredscontent = new FormUrlEncodedContent(logincreds.ToKeyValue());
                var result = this.client.PostAsync(oauthurl,logincredscontent).Result;
                if (result.StatusCode == HttpStatusCode.BadRequest || result.StatusCode == HttpStatusCode.Unauthorized || result.StatusCode == HttpStatusCode.Forbidden)
                {
                    var message = JsonConvert.DeserializeObject<SecretServerTokenResponse>(result.Content.ReadAsStringAsync().Result);
                    throw new Exception("BadRequest: " + message.error);
                }
                var content = JsonConvert.DeserializeObject<SecretServerTokenResponse>(result.Content.ReadAsStringAsync().Result);
                // this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", content.access_token);
                try
                {
                    this.client.DefaultRequestHeaders.Remove("Authorization");
                }
                catch
                {
                }
                this.tokenexpires = DateTime.Now.AddMinutes(content.expires_in);
                if (_logs.IsDebug()) _logs.NewLog("Token expires in mins: " + content.expires_in.ToString() + " at " + this.tokenexpires.ToString());
                this.client.DefaultRequestHeaders.Add("Authorization","Bearer " + content.access_token); //= new AuthenticationHeaderValue("Bearer", content.access_token);
                return true;
            }
            catch (Exception ex)
            {
                _logs.NewLog(ex,"SecretServer Login: " + ex.Message + ex.StackTrace);
                return false;
            }
        }

        private bool Logout()
        {
            return true;
        }

        public List<SecretServerSecretSummary> GetSecrets()
        {
            Login();
            string secretsurl = _settings.GetSettingValue("secretserversecretsurl"),
                apiurl = _settings.GetSettingValue("secretserverapiurl");

            try
            {
                var response = this.client.GetStringAsync(apiurl + "/" + secretsurl).Result;
                SecretServerPagingSecretSummary pagingsecrets = JsonConvert.DeserializeObject<SecretServerPagingSecretSummary>(response);
                return pagingsecrets.records;
            }
            catch (Exception e)
            {
                _logs.NewLog(e,"Get Secrets error: " + e.Message);
                return new List<SecretServerSecretSummary>();
            }
        }

        public SecretServerSecret GetSecret(int id)
        {
            Login();
            string secreturl = _settings.GetSettingValue("secretserversecreturl"),
                apiurl = _settings.GetSettingValue("secretserverapiurl");

            try
            {
                var response = this.client.GetStringAsync(apiurl + "/" + String.Format(secreturl,id)).Result;
                SecretServerSecret secret = JsonConvert.DeserializeObject<SecretServerSecret>(response);
                this.lastsecret = secret;
                return this.lastsecret;
            }
            catch (Exception e)
            {
                _logs.NewLog(e,"Get Secret error: " + e.Message);
                return new SecretServerSecret();
            }
        }

        public string GetSecretUsername(int id)
        {
            try
            {
                SecretServerSecret secret = this.lastsecret != null && this.lastsecret.id == id ? this.lastsecret : GetSecret(id);
                return secret.items.Find(x => x.slug == "username").itemValue;
            }
            catch (Exception ex)
            {
                _logs.NewLog(ex,"GetSecretUsername error: " + ex.Message);
                return "";
            }
        }

        public string GetSecretPassword(int id)
        {
            try
            {
                SecretServerSecret secret = this.lastsecret != null && this.lastsecret.id == id ? this.lastsecret : GetSecret(id);
                return secret.items.Find(x => x.slug == "password").itemValue;
            }
            catch (Exception ex)
            {
                _logs.NewLog(ex,"GetSecretPassword error: " + ex.Message);
                return "";
            }
        }
    }
}