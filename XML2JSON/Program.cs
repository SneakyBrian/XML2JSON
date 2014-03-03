using System.IO;
using XML2JSON.Core;
using System;
using System.Reflection;

namespace XML2JSON
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().FullName);

            var inputXml = args[0];
            var outputJson = args[1];

            Console.WriteLine("input xml: {0}", inputXml);
            Console.WriteLine("output json: {0}", outputJson);

            string xml;

            using (var inputFileStream = File.OpenRead(inputXml))
            {
                using (var inputStreamReader = new StreamReader(inputFileStream))
                {
                    xml = inputStreamReader.ReadToEnd();
                }
            }

            Console.WriteLine("Loaded input xml from '{0}'", inputXml);

            var json = Converter.ConvertToJson(xml);

            Console.WriteLine("Converted xml to json");

            //save out
            using (var outputFileStream = File.Open(outputJson, FileMode.Create, FileAccess.Write))
            {
                using (var outputStreamWriter = new StreamWriter(outputFileStream))
                {
                    outputStreamWriter.Write(json);
                    outputStreamWriter.Flush();
                    outputStreamWriter.Close();
                }
            }

            Console.WriteLine("Saved output json to '{0}'", outputJson);
        }
    }
}