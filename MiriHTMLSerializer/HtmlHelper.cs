using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiriHTMLSerializer
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper instance = new HtmlHelper();
        public static HtmlHelper Instance => instance;

        public string[] JsonHtmlTags { get; set; }
        public string[] JsonHtmlSelfClosingTags { get; set; }
        private HtmlHelper()
        {
            var htmlTags = File.ReadAllText("AttechFiles/HtmlTags.json");
            JsonHtmlTags = JsonSerializer.Deserialize<string[]>(htmlTags);
            var htmlSelfClosingTags = File.ReadAllText("AttechFiles/HtmlVoidTags.json");
            JsonHtmlSelfClosingTags = JsonSerializer.Deserialize<string[]>(htmlSelfClosingTags);
        }
    }
}
