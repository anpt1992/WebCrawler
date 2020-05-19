using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Crawlers
{
    public class MSNCrawler : ICrawler
    {
        public MSNCrawler() { }
        public List<string> GetPages()
        {
            List<string> pages = new List<string>();
            pages.Add("https://www.msn.com/en-za");



            return pages;
        }
        public List<News> GetItemsPages()
        {
            string urlTarget = "https://www.msn.com/en-za";
            List<News> items = new List<News>();

            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36",
                OverrideEncoding = Encoding.UTF8
            };

            HtmlDocument document = htmlWeb.Load(urlTarget);

            RemoveNode(document, "//div[@data-section-id='stripe.video']");
            RemoveNode(document, "//div[@data-section-id='stripe.photos']");

            var smallaItems = document.DocumentNode.QuerySelectorAll(".smalla").ToList();
            foreach (var item in smallaItems)
            {
                if (item.GetClasses().Contains("video") || item.GetClasses().Contains("photo"))
                {
                    continue;
                }
                var news = new News();
                var linkNode = item.QuerySelector("a");
                var link = WraperUrl(linkNode.Attributes["href"].Value);
                news.sourceLink = link;
                var title = linkNode.QuerySelector(".caption > .title").InnerText;
                news.title = title;
                items.Add(news);
            }

            //var mediumaItems = document.DocumentNode.QuerySelectorAll(".mediuma").ToList();
            //foreach (var item in mediumaItems)
            //{
            //    if (item.GetClasses().Contains("video") || item.GetClasses().Contains("photo"))
            //    {
            //        continue;
            //    }
            //    var news = new News();
            //    var linkNode = item.QuerySelector("a");
            //    var link = WraperUrl(linkNode.Attributes["href"].Value);
            //    news.sourceLink = link;
            //    var title = linkNode.QuerySelector(".caption > .title").InnerText;
            //    news.title = title;
            //    items.Add(news);
            //}

            //var hasimageItems = document.DocumentNode.QuerySelectorAll(".swipenav > .hasimage").ToList();
            //foreach (var item in hasimageItems)
            //{
            //    if (item.GetClasses().Contains("video") || item.GetClasses().Contains("photo"))
            //    {
            //        continue;
            //    }
            //    var news = new News();
            //    var link = WraperUrl(item.QuerySelector("a").Attributes["href"].Value);
            //    news.sourceLink = link;
            //    var title = item.QuerySelector(".caption > .title").InnerText;
            //    news.title = title;
            //    items.Add(news);
            //}

            //var tertiaryItems = document.DocumentNode.QuerySelectorAll(".paging-container > .stripecontent > .tertiary > li").ToList();
            //foreach (var item in tertiaryItems)
            //{
            //    var news = new News();
            //    var link = WraperUrl(item.QuerySelector("a").Attributes["href"].Value);
            //    if (item.GetClasses().Contains("video") || link.Contains("/lifestyle/horoscope"))
            //    {
            //        continue;
            //    }
            //    news.sourceLink = link;
            //    items.Add(news);
            //}

            //var secondaryItems = document.DocumentNode.QuerySelectorAll(".paging-container > .stripecontent > .secondary > li").ToList();
            //foreach (var item in secondaryItems)
            //{
            //    if (item.GetClasses().Contains("video") || item.GetClasses().Contains("photo"))
            //    {
            //        continue;
            //    }
            //    var news = new News();
            //    var link = WraperUrl(item.QuerySelector("a").Attributes["href"].Value);
            //    news.sourceLink = link;
            //    items.Add(news);
            //}

            var newlistItems = document.DocumentNode.QuerySelectorAll(".newlist > * li").ToList();
            foreach (var item in newlistItems)
            {
                if (item.GetClasses().Contains("video") || item.GetClasses().Contains("photo"))
                {
                    continue;
                }
                var news = new News();
                var link = WraperUrl(item.QuerySelector("a").Attributes["href"].Value);
                news.sourceLink = link;
                items.Add(news);
            }

            return items;
        }
        public News ReadItem(News news)
        {
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36",
                OverrideEncoding = Encoding.UTF8
            };
            HtmlDocument document = htmlWeb.Load(news.sourceLink);
            news.isEn = true;
            if (news.title == null)
            {
                news.title = document.DocumentNode.QuerySelector(".collection-headline-flex > h1").InnerText.Trim();
            }
            if (news.thumb == null)
            {                
                news.thumb = document.DocumentNode.SelectSingleNode("//meta[@property='og:image']").GetAttributeValue("content",null);
            }      
            string strCreatedDate = document.DocumentNode.QuerySelector("time").GetAttributeValue("datetime", null);

            news.createdDate = DateTime.ParseExact(strCreatedDate, "yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture);
            news.publishedDate = news.createdDate;

            return news;
        }
        private string WraperUrl(string input)
        {
            if (input.StartsWith("/en-za"))
            {
                input = "https://www.msn.com" + input;
                int index = input.IndexOf("?");
                if (index > 0)
                    input = input.Substring(0, index);
            }
            else if (input.StartsWith("//static"))
            {
                input = "https:" + input;
            }
            return input;
        }

        private void RemoveNode(HtmlDocument document, string xpath)
        {
            var div = document.DocumentNode.SelectSingleNode(xpath);
            foreach (HtmlNode node in div.SelectNodes("*"))
            {
                node.Remove();
            }
        }
    }
}
