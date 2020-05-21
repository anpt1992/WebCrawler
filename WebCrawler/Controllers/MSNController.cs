using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebCrawler.Crawlers;
using WebCrawler.Services;

namespace WebCrawler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MSNController : ControllerBase
    {
        private readonly NewsService _newsService;

        public MSNController(NewsService newsService)
        {
            _newsService = newsService;
        }
        public string Index()
        {
            MSNCrawler msn = new MSNCrawler();
            Dictionary<string, string> pages = msn.GetPages();
            foreach (var page in pages)
            {
                var items = msn.GetItemsPages(page.Key,page.Value);
                items = items.Select(item => msn.ReadItem(item)).ToList();
                _newsService.SaveAll(items);
            }
            return "This is my default action...";
        }
    }
}