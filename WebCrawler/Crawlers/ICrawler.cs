using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler.Crawlers
{
    interface ICrawler
    {
        List<string> GetPages();
    }
}
