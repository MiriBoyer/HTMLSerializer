using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiriHTMLSerializer
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var item in current.Children)
                {
                    queue.Enqueue(item);
                }
                yield return current;
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = this;
            while(current!=null)
            {
                yield return current;
                current = current.Parent;
            }
        }
    }
}
