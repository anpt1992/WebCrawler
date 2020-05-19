using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebCrawler.Crawlers;

namespace WebCrawler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MSNController : ControllerBase
    {
        public string Index()
        {
            MSNCrawler msn = new MSNCrawler();
            Dictionary<string, string> pages = msn.GetPages();
            foreach (var page in pages)
            {
                var items = msn.GetItemsPages(page.Key,page.Value);
                items = items.Select(item => msn.ReadItem(item)).ToList();
            }
            return "This is my default action...";
        }
    }
}