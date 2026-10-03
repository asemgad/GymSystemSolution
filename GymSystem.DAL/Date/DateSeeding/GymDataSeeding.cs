using Microsoft.EntityFrameworkCore.Storage.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymSystem.DAL.Date.DateSeeding
{
    public static class GymDataSeeding
    {





        private static List<T> LoadDateFromJsonFile <T>(string folderPath , string fileName)
        {
            var path = Path.Combine(folderPath, fileName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"seed date file is not found ${fileName}");
            }

            var data = File.ReadAllText(path);

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            ;
            return JsonSerializer.Deserialize<List<T>>(data, options) ?? [];
        }
    }
}
