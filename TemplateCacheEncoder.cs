using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace solomon87.DotNetAngularTemplateCacheGenerator
{
    public class TemplateCacheEncoder
    {
        public static string GenerateTemplateCache(string path)
        {

            var templateBuilder = new StringBuilder();

            // Get all partial.html and template.html files within the "path" directory
            var partialHtmlFiles = Directory.EnumerateFiles(path, "*.partial.html", SearchOption.AllDirectories).ToList();
                partialHtmlFiles.AddRange(Directory.EnumerateFiles(path, "*.template.html", SearchOption.AllDirectories));

            foreach (var partialPath in partialHtmlFiles)
            {
                using (var stream = new StreamReader(partialPath))
                {
                    templateBuilder.Append(string.Format(
                        "$templateCache.put('{0}', '{1}');{2}"
                        , Path.GetFileName(partialPath)
                        , HttpUtility.JavaScriptStringEncode(stream.ReadToEnd())
                        , Environment.NewLine));
                }
            }

            return Clean(templateBuilder.ToString());
        }

        private static string Clean(string str)
        {
            // Get rid of any CR/LF pairs and trailing whitespace, and remove any comment sections
            var regex1 = new Regex(@"\\r\\n\s*");
            var regex2 = new Regex(@"\\u003c!--.*--\\u003e");
            str = regex1.Replace(str, "");
            str = regex2.Replace(str, "");
            return str;
        }

    }
}