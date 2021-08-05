using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NyikoConsoleApp
{

    class ApiClient
    {
        private readonly string _baseUri;
        private readonly string _dataType;
        private readonly bool _enabled;

        public ApiClient(string baseUri, string dataType, bool enabled)
        {
            this._baseUri = baseUri;
            this._dataType = dataType;
            this._enabled = enabled;
        }

        public async Task<R_type> DeserializeJSON<R_type>(string resource)
        {
            using (var _httpClient = new HttpClient())
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Clear();

                    HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(_baseUri + resource);
                    httpResponseMessage.EnsureSuccessStatusCode();

                    if (httpResponseMessage.IsSuccessStatusCode && _dataType == "JSON" && _enabled == true)
                    {

                        var response = httpResponseMessage.Content.ReadAsStringAsync().Result;

                        return JsonConvert.DeserializeObject<R_type>(response);
                    }

                    else
                    {
                        throw new Exception("Connection failed");
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


    }
    class ApiClientXML
    {
        private readonly string _baseUri;
        private readonly string _dataType;
        private readonly bool _enabled;

        public ApiClientXML(string baseUri, string dataType, bool enabled)
        {
            this._baseUri = baseUri;
            this._dataType = dataType;
            this._enabled = enabled;
        }
        public async Task<T> DeserializeXml<T>(string resource) where T : class
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                T response = null;
                var _httpClient = new HttpClient();
                using (TextReader reader = new StringReader(resource))
                {
                    _httpClient.DefaultRequestHeaders.Clear();
                    HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(_baseUri + resource);
                    httpResponseMessage.EnsureSuccessStatusCode();
                    if (httpResponseMessage.IsSuccessStatusCode && _dataType == "XML" && _enabled == true)
                    {
                        var res = httpResponseMessage.Content.ReadAsStringAsync().Result;

                        response = (T)serializer.Deserialize(reader);
                    }
                    else
                    {
                        throw new Exception("Connection failed");
                    }

                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}


