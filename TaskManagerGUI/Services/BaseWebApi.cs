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
            try
            {
                var response = await GetResponseMessage(url);
                if(response != null)
                {
                    return await response.Content.ReadFromJsonAsync<T>();
                }
                return default(T);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }
        protected async Task<T> GetInfoNonClass<T>(string url) where T: struct
        {
            try
            {
                var response = await GetResponseMessage(url);
                if (response != null)
                {
                    return (T)Convert.ChangeType(await response.Content.ReadAsStringAsync(), typeof(T));
                }
                return default(T);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }
        protected async Task<string> GetInfoString(string url)
        {
            try
            {
                var response = await GetResponseMessage(url);
                if (response != null)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }


        protected async Task<Recieve> PostInfo<Send, Recieve>(string url, Send item) where Send : class where Recieve : struct
        {
            try
            {
                var response = await _http.PostAsJsonAsync(url, item);
                if(response.IsSuccessStatusCode) {
                    return (Recieve)Convert.ChangeType(await response.Content.ReadAsByteArrayAsync(), typeof(Recieve));
                }
                return default;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }

        }
        protected async Task<Recieve> PostInfoClass<Send, Recieve>(string url, Send item) where Send : class where Recieve : class
        {
            try
            {
                var response = await _http.PostAsJsonAsync(url, item);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Recieve>();
                }
                return default;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }

        }
        protected async Task<string> PostInfoString<Send, Recieve>(string url, Send item) where Send : class where Recieve : class
        {
            try
            {
                var response = await _http.PostAsJsonAsync(url, item);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }

        }

        #endregion Protected Methods


        #region Protected Methods
        private async Task<HttpResponseMessage> GetResponseMessage(string url)
        {
            try
            {
                var response = await _http.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                return null;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        #endregion Private Methods


    }
}
