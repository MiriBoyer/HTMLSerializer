using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiriHTMLSerializer
{
    public static class Extension
    {
        static IEnumerable<HtmlElement> ElementsBySelectors(this HtmlElement element,Selector selector)
        {
            var elementsList = element.Descendants(element);
            foreach ( var ele in elementsList )
            {
                if (selector.Child == null)
                {
                    yield return ele;
                }
                else
                if (selector.Equals(ele))
                {
                    foreach ( var ele2 in ele.ElementsBySelectors(selector.Child))
                    {
                        yield return ele2;
                    }
                }
            }
        }
    }
}
