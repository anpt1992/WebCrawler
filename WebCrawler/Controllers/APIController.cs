﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebCrawler.Models;
using WebCrawler.Services;

namespace WebCrawler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly NewsService _newsService;
        private readonly IMapper _mapper;

        public APIController(NewsService newsService, IMapper mapper)
        {
            _newsService = newsService;
            _mapper = mapper;
        }

        [HttpGet("en/get_arc_by_catid")]       
        public ActionResult<List<NewsByCategory>> GetArcByCategory([FromQuery] string catid)
        {
            var news = _newsService.GetByCategory(catid);

            if (news == null)
            {
                return NotFound();
            }

            return _mapper.Map<List<News>, List<NewsByCategory>>(news);
        }

        [HttpGet("en/get_noi_dung_tin_new")]      
        public ActionResult GetArc([FromQuery] string arcid)
        {
            var news = _newsService.Get(arcid);

            if (news == null)
            {
                return NotFound();
            }
            //   return news;
            return this.Content(news.contentHtml,"text/html");
        }
        
    }
}