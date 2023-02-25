using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiaryProject.Data
{
    public class DataSerializer<T>
    {
        private readonly string _filePath;

        public DataSerializer(string filePath)
        {
            _filePath = filePath;
        }

        public List<T> Deserialize()
        {
            List<T> result = new List<T>();
            if (File.Exists(_filePath))
            {
                string data = File.ReadAllText(_filePath);
                result = JsonConvert.DeserializeObject<List<T>>(data);
            }
            return result;
        }

        public void Serialize(List<T> data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            File.WriteAllText(_filePath, jsonData);
        }
    }

}
