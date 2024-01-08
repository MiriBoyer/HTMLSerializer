using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiriHTMLSerializer
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }
        public static Selector CastToSelector(string str)
        {
            var splitToLevels = str.Split(" ");
            Selector root = new Selector(), temp = root, parent = null;
            for (int i = 0; i < splitToLevels.Length; i++)
            {
                var level = splitToLevels[i].Trim();
                var idInd = level.IndexOf('#');
                var classInd = level.IndexOf('.');
                if (idInd != -1)
                {
                    if (classInd > idInd)
                        temp.Id = level.Substring(idInd + 1, classInd - idInd - 1);
                    else
                        temp.Id = level.Substring(idInd + 1);
                }
                if (classInd != -1)
                {
                    temp.Classes = level.Substring(classInd + 1, idInd > classInd ? idInd - classInd - 1 : level.Length - classInd-1).Split(" ").ToList();
                }
                string name = "";
                if (level[0] != '#' && level[0] != '.')
                {
                    if (classInd == -1 && idInd == -1)
                        name = level;
                    else
                    {
                        if (classInd == -1)
                            name = level.Substring(0, idInd);
                        else
                        {
                            if (idInd == -1)
                                name = level.Substring(0, classInd);
                            else
                                name = level.Substring(0, Math.Min(idInd, classInd));
                        }
                    }
                }
                if (name != "" && HtmlHelper.Instance.JsonHtmlTags.Contains(name))
                    temp.TagName = name;
                temp.Parent = parent;
                parent = temp;
                if (i < splitToLevels.Length - 1)
                {
                    temp.Child = new Selector();
                    temp = temp.Child;
                }
            }
            return root;
        }
        public override bool Equals(object? obj)
        {
            if (obj is HtmlElement)
            {
                HtmlElement element = obj as HtmlElement;
                if (element != null)
                {
                    bool isClasses = true;
                    if (Classes != null)
                        foreach (var c in Classes)
                        {
                            if (!element.Classes.Contains(c))
                                isClasses = false;
                        }
                    return (TagName == null || element.Name.Equals(TagName)) && (Id == null ||( element.Id!=null&&element.Id.Equals(Id))) && isClasses;
                }
            }
            return false;
        }
    }
}
