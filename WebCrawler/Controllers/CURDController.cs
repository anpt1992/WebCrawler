using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebCrawler.Models;
using WebCrawler.Services;

namespace WebCrawler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CURDController : ControllerBase
    {
        private readonly NewsService _newsService;
      
        public CURDController(NewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        public ActionResult<List<News>> Get() =>
            _newsService.Get();

        [HttpGet("{id:length(24)}", Name = "GetNews")]
        public ActionResult<News> Get(string id)
        {
            var news = _newsService.Get(id);

            if (news == null)
            {
                return NotFound();
            }

            return news;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<News> Create([FromBody] News news)
        {
            _newsService.Create(news);

            return CreatedAtRoute("GetNews", new { id = news.id.ToString() }, news);
        }
    }
}