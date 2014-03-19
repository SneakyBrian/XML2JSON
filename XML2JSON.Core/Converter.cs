using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace XML2JSON.Core
{
    /// <summary>
    /// Class to handle xml -> json conversion
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// converts xml string to json string
        /// </summary>
        /// <param name="xml">xml data as string</param>
        /// <returns>json data as string</returns>
        public static string ConvertToJson(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

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

            return strippedJsonText;
        }

        /// <summary>
        /// async version of conversion function
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static async Task<string> ConvertToJsonAsync(string xml)
        {
            return await Task<string>.Factory.StartNew(() => ConvertToJson(xml));
        }
    }
}
