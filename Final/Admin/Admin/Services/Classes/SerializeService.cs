using Admin.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Admin.Services.Classes
{
    public class SerializeService : ISerializeService
    {
        public T Deserialize<T>(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json);
            }

            catch (Exception)
            {

                throw;
            }
        }

        public string Serialize<T>(object? obj)
        {
            if (obj != null)
            {
                return JsonSerializer.Serialize(obj);
            }

            throw new NullReferenceException("Object is null");
        }
    }
}