using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.EntityFramework.Scaffolding
{
    public static class TemplateContextHelper
    {
        public static string GetTemplate()
        {
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TemplateDbContext.cs");

            using (FileStream stream = new FileStream(filepath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string context = reader.ReadToEnd();
                    return context;
                }
            }
        }

        public static void SaveTemplate(string content, string filepath)
        {
            using (FileStream stream = new FileStream(filepath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(content);
                }
            }
        }
    }
}
