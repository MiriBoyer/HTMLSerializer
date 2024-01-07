// See https://aka.ms/new-console-template for more information
using MiriHTMLSerializer;
using System.Text.RegularExpressions;
HtmlElement root=new HtmlElement(); ;
Stack<int> stack = new Stack<int>();
var html = await Load("https://hebrewbooks.org/beis");
var cleanEmptyLines = new Regex("\\n|\\r").Replace(html, "");
var splitHtmlToLines = new Regex("<(.*?)>").Split(cleanEmptyLines).Where(s => s.Trim().Length > 0);
var splitHtmlToLinesInArray = splitHtmlToLines.ToArray();
HtmlElement[] elements = new HtmlElement[splitHtmlToLinesInArray.Length];
int currentSize = -1;
for (int i = 0; i < splitHtmlToLinesInArray.Length; i++)
{
    var temp = splitHtmlToLinesInArray[i].Trim();
    var splitSingleLine = temp.Split(" ");
    if (temp.Trim().Length > 0)
    {
        var TagName = splitSingleLine[0];
        if (HtmlHelper.Instance.JsonHtmlTags.Contains(TagName)&&TagName!="var")
        {
            elements[++currentSize] = new HtmlElement();            
            if (currentSize == 0) root = elements[currentSize];
            elements[currentSize].Name = TagName;
            elements[currentSize].Parent = currentSize > 0 ? elements[currentSize - 1] : null;
            elements[currentSize].InnerHtml = "";
            elements[currentSize].Attributes = new List<string>();
            elements[currentSize].Classes = new List<string>();
            elements[currentSize].Children = new List<HtmlElement>();
            var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(temp).ToList();
            var id = new Regex("id=\"(.*?)\"").Matches(temp).ToList();
            var classes = new Regex("class=\"(.*?)\"").Matches(temp).ToList();
            if (id.Count > 0)
                elements[currentSize].Id = id.ElementAt(0).Value;
            foreach (var attr in attributes)
            {
                elements[currentSize].Attributes.Add(attr.Value);
            }
            foreach (var item in classes)
            {
                elements[currentSize].Classes.Add(item.Value);
            }

            if (HtmlHelper.Instance.JsonHtmlSelfClosingTags.Contains(TagName))
            {
                if (currentSize > 0)
                    elements[currentSize - 1].Children.Add(elements[currentSize]);
                currentSize--;
            }
        }
        else
        {
            var closeTag = splitSingleLine[splitSingleLine.Length - 1];
            var closeTagEnd="";
            if (closeTag.Length >= 2)
            {
            closeTagEnd= closeTag.Substring(0, 2);
                closeTag = closeTag.Substring(2);

            }
            if (HtmlHelper.Instance.JsonHtmlTags.Contains(TagName.Substring(1)) && TagName[0] == '/' ||
                HtmlHelper.Instance.JsonHtmlTags.Contains(closeTag) &&  closeTagEnd== "</")
            {
                if (elements[currentSize].Name != TagName.Substring(1)&&closeTag != elements[currentSize].Name)
                {
                    Console.WriteLine("ERROR: the HTML syntax is not good, in line" + i);
                }
                else
                {
                    if (currentSize > 0)
                        elements[currentSize - 1].Children.Add(elements[currentSize]);
                    currentSize--;
                }

            }
            else
            {
                if (currentSize > -1)
                    elements[currentSize].InnerHtml += temp;
            }
        }
    }
}

var li=root.ElementsBySelectors(Selector.CastToSelector("#form1"),true);
var mul =new HashSet<HtmlElement>(li);
Console.WriteLine(mul.Count());
foreach (var item in mul)
{
    Console.WriteLine(item.Name);
}
Console.ReadLine();
async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
string str(int i)
{
    string s = "";
    for (int j = 0; j < i; j++)
    {
        s += "\t";
    }
    return s;
}