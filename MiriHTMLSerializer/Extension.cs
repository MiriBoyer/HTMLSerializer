using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiriHTMLSerializer
{
    public static class Extension
    {
        public static IEnumerable<HtmlElement> ElementsBySelectors(this HtmlElement element, Selector selector, bool b = false)
        {
            if (b)
            {
                var elementsList = element.Descendants();
                foreach (var element2 in elementsList)
                    if (selector.Equals(element2))
                    {
                        var list = element2.ElementsBySelectors(selector);
                        foreach (var item in list)
                            yield return item;
                    }
            }
            if (selector.Equals(element))
            {
                if (selector.Child == null)
                    yield return element;
                else
                {
                    var elementsList = element.Descendants();
                    selector = selector.Child;
                    foreach (var element2 in elementsList)
                        if (selector.Equals(element2))
                        {
                            var list = element2.ElementsBySelectors(selector, false);
                            foreach (var item in list)
                                yield return item;
                        }
                }
            }
        }
    }
}
