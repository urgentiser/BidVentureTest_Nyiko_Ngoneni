using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NyikoConsoleApp.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NyikoConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
           


            var fileData = await FileHelper.Read(@"C:\Users\User\Downloads\BitVenture_-_Coding_Project\bonus_endpoints.json");

            var _fileContent = JObject.Parse(fileData);


            var services = _fileContent.ToObject<Root>(); 


            foreach(var service in services.services)
            {
                ApiClient clientJSON = new ApiClient(service.baseURL, service.datatype, service.enabled);
                ApiClientXML clientXML = new ApiClientXML(service.baseURL, service.datatype, service.enabled);
                foreach (var _endpoint in service.endpoints)
                {
                   
                    if(service.datatype=="JSON")
                    {
                        var resp = await clientJSON.DeserializeJSON<dynamic>(_endpoint.resource);
                        Console.WriteLine("Info: {0}", resp);

                        Console.WriteLine();
                    }
                    else if (service.datatype == "XML")
                    {
                        var response = await clientXML.DeserializeXml<dynamic>(_endpoint.resource);
                        Console.WriteLine("Info: {0}", response);

                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine(ErrorEventArgs.Empty);
                    }

                }
            }
           

        }
    }

    class FileHelper 
    {
        public static async Task<string> Read(string fullPath)
        {
            try
            {
                var fileContent = await File.ReadAllTextAsync(fullPath);
                return fileContent;
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
        }

    }
}
