using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace XML2JSON
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputXml = args[0];
            var outputJson = args[1];

            var doc = new XmlDocument();
            doc.Load(inputXml);

            //strip comments from xml
            var comments = doc.SelectNodes("//comment()");

            if (comments != null)
            {
                foreach (var node in comments.Cast<XmlNode>())
                {
                    if (node.ParentNode != null)
                        node.ParentNode.RemoveChild(node);
                }
            }

            var rawJsonText = JsonConvert.SerializeXmlNode(doc.DocumentElement, Formatting.Indented);

            //strip the @ and # characters
            var strippedJsonText = Regex.Replace(rawJsonText, "(?<=\")(@)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);
            strippedJsonText = Regex.Replace(strippedJsonText, "(?<=\")(#)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);

            //save out
            using (var outputFileStream = File.Open(outputJson, FileMode.Create, FileAccess.Write))
            {
                using (var outputStreamWriter = new StreamWriter(outputFileStream))
                {
                    outputStreamWriter.Write(strippedJsonText);
                    outputStreamWriter.Flush();
                    outputStreamWriter.Close();
                }
            }
        }
    }
}