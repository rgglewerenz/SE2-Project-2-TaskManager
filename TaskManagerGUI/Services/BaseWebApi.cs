using Microsoft.AspNetCore.Mvc.Formatters;
using Quobject.SocketIoClientDotNet.Client;
using System.Web;
using TaskManagerGUI.Interface;

namespace TaskManagerGUI.Services
{
    public class BaseWebApi : IBaseWebApi
    {

        public IConfiguration _config { get; set; }
        protected HttpClient _http { get; set; }
        private string _url;
        protected string BaseURL { get
            {
                return _url;
            } set
            {
                _url = value;
            }
        }

        public BaseWebApi(HttpClient http, IConfiguration config)
        {
            _config = config;
            _http = http;
            _url = "";
        }


        #region Protected Methods
        protected async Task<T> GetInfoFromJson<T>(string url) where T : class
        {
            var response = await GetResponseMessage(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            Exception? ServerEx;
            try
            {
                string item = await response?.Content.ReadAsStringAsync();
                ServerEx = new Exception(item.Split("\n")[0].Split("System.Exception:")[1]);
            }
            catch (Exception ex)
            {
                return default(T);
            }
            throw ServerEx ?? new Exception("Unknown Error");
        }


        protected async Task<T> GetInfoNonClass<T>(string url) where T: struct
        {
            var response = await GetResponseMessage(url);
            if (response.IsSuccessStatusCode)
            {
               return (T)Convert.ChangeType(await response.Content.ReadAsStringAsync(), typeof(T));
            }
            Exception? ServerEx;
            try
            {
                string item = await response?.Content.ReadAsStringAsync();
                ServerEx = new Exception(item.Split("\n")[0].Split("System.Exception:")[1]);
            }
            catch(Exception ex)
            {
                return default(T);
            }
            throw ServerEx ?? new Exception("Unknown Error");
            
        }
        protected async Task<string> GetInfoString(string url)
        {
            var response = await GetResponseMessage(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            Exception? ServerEx;
            try
            {
                string item = await response?.Content.ReadAsStringAsync();
                ServerEx = new Exception(item.Split("\n")[0].Split("System.Exception:")[1]);
            }
            catch (Exception ex)
            {
                return "";
            }
            throw ServerEx ?? new Exception("Unknown Error");
        }

        protected async Task<Recieve> PostInfo<Send, Recieve>(string url, Send item) where Send : class where Recieve : struct
        {
            var response = await _http.PostAsJsonAsync(url, item);
            if (response.IsSuccessStatusCode)
            {
                return (Recieve)Convert.ChangeType(await response.Content.ReadAsStringAsync(), typeof(Recieve));
            }
            Exception? ServerEx;
            try
            {
                string str = await response?.Content.ReadAsStringAsync();
                ServerEx = new Exception(str.Split("\n")[0].Split("System.Exception:")[1]);
            }
            catch (Exception ex)
            {
                return default;
            }
            throw ServerEx ?? new Exception("Unknown Error");
        }
        protected async Task<Recieve> PostInfoClass<Send, Recieve>(string url, Send item) where Send : class where Recieve : class
        {
            var response = await _http.PostAsJsonAsync(url, item);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Recieve>();
            }
            Exception? ServerEx;
            try
            {
                string str = await response?.Content.ReadAsStringAsync();
                ServerEx = new Exception(str.Split("\n")[0].Split("System.Exception:")[1]);
            }
            catch (Exception ex)
            {
                return default(Recieve);
            }
            throw ServerEx ?? new Exception("Unknown Error");
        }
        protected async Task<string> PostInfoString<Send, Recieve>(string url, Send item) where Send : class where Recieve : class
        {
            var response = await _http.PostAsJsonAsync(url, item);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            Exception? ServerEx;
            try
            {
                string str = await response?.Content.ReadAsStringAsync();
                ServerEx = new Exception(str.Split("\n")[0].Split("System.Exception:")[1]);
            }
            catch (Exception ex)
            {
                return "";
            }
            throw ServerEx ?? new Exception("Unknown Error");
        }

        #endregion Protected Methods


        #region Protected Methods
        private async Task<HttpResponseMessage> GetResponseMessage(string url)
        {
            try
            {
                var response = await _http.GetAsync(url);
                return response;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadGateway };
            }
        }
        #endregion Private Methods


    }
}
