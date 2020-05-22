using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Crawlers
{
    public class MSNCrawler : ICrawler
    {
        public MSNCrawler() { }
        public Dictionary<string, string> GetPages()
        {
            var pages = new Dictionary<string, string>();
            pages.Add("0", "https://www.msn.com/en-za");

            return pages;
        }
        public List<News> GetItemsPages(string catId, string urlTarget)
        {
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36",
                OverrideEncoding = Encoding.UTF8
            };

            HtmlDocument document = htmlWeb.Load(urlTarget);

            var items = GetHomePage(document, catId);

            return items;
        }
        public List<News> GetHomePage(HtmlDocument document, string catId)
        {
            List<News> items = new List<News>();

         
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
                var eSource = linkNode.QuerySelectorAll(".caption > .sourcename > img");
                if (!(eSource.Count() == 0))
                {
                    var source = eSource.First().GetAttributeValue("alt", "");
                    news.source = source.Substring(0, source.Length - 5);
                }
                news.catid = catId;
                items.Add(news);
            }

            var mediumaItems = document.DocumentNode.QuerySelectorAll(".mediuma").ToList();
            foreach (var item in mediumaItems)
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
                var eSource = linkNode.QuerySelectorAll(".caption > .sourcename > img");
                if (!(eSource.Count() == 0))
                {
                    var source = eSource.First().GetAttributeValue("alt", "");
                    news.source = source.Substring(0, source.Length - 5);
                }
                news.catid = catId;
                items.Add(news);
            }

            var hasimageItems = document.DocumentNode.QuerySelectorAll(".swipenav > .hasimage").ToList();
            foreach (var item in hasimageItems)
            {
                var link = WraperUrl(item.QuerySelector("a").Attributes["href"].Value);
                if (item.GetClasses().Contains("video") || item.GetClasses().Contains("photo") || !link.Contains("/en-za/"))
                {
                    continue;
                }
                var news = new News();
                news.sourceLink = link;
                var title = item.QuerySelector(".caption > .title").InnerText;
                news.title = title;
                var eSource = item.QuerySelectorAll(".caption > .sourcename > img");
                if (!(eSource.Count() == 0))
                {
                    var source = eSource.First().GetAttributeValue("alt", "");
                    news.source = source.Substring(0, source.Length - 5);
                }
                news.catid = catId;
                items.Add(news);
            }

            var tertiaryItems = document.DocumentNode.QuerySelectorAll(".paging-container > .stripecontent > .tertiary > li").ToList();
            foreach (var item in tertiaryItems)
            {
                var news = new News();
                var link = WraperUrl(item.QuerySelector("a").Attributes["href"].Value);
                if (item.GetClasses().Contains("video") || item.GetClasses().Contains("photo") || link.Contains("/lifestyle/horoscope"))
                {
                    continue;
                }
                news.sourceLink = link;
                var eSource = item.QuerySelectorAll("a > div > img");
                if (!(eSource.Count() == 0))
                {
                    var source = eSource.First().GetAttributeValue("alt", "");
                    news.source = source.Substring(0, source.Length - 5);
                }
                news.catid = catId;
                items.Add(news);
            }

            var secondaryItems = document.DocumentNode.QuerySelectorAll(".paging-container > .stripecontent > .secondary > li").ToList();
            foreach (var item in secondaryItems)
            {
                var link = WraperUrl(item.QuerySelector("a").Attributes["href"].Value);
                if (item.GetClasses().Contains("video") || item.GetClasses().Contains("photo") || link.Contains("/lifestyle/horoscope"))
                {
                    continue;
                }
                var news = new News();
                news.sourceLink = link;
                var eSource = item.QuerySelectorAll("a > div > img");
                if (!(eSource.Count() == 0))
                {
                    var source = eSource.First().GetAttributeValue("alt", "");
                    news.source = source.Substring(0, source.Length - 5);
                }
                news.catid = catId;
                items.Add(news);
            }

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
                var source = item.QuerySelector("a > .sourcename > img").GetAttributeValue("alt", "");
                news.source = source.Substring(0, source.Length - 5);
                news.catid = catId;
                items.Add(news);
            }
            return items;
        }
        public News ReadItem(News news)
        {
            try
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
                    news.title = document.DocumentNode.SelectSingleNode("//meta[@property='og:title']").GetAttributeValue("content", null);
                }
                if (news.thumb == null)
                {
                    news.thumb = document.DocumentNode.SelectSingleNode("//meta[@property='og:image']").GetAttributeValue("content", null);
                }
                string strCreatedDate = document.DocumentNode.QuerySelector("time").GetAttributeValue("datetime", null);

                news.createdDate = DateTime.ParseExact(strCreatedDate, "yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture);
                news.publishedDate = news.createdDate;

                List<string> removeNodes = new List<string>();
                removeNodes.Add("strong");
                removeNodes.Add("script");
                removeNodes.Add(".readmore");
                foreach(var node in removeNodes)
                {
                    var elements = document.DocumentNode.QuerySelectorAll(node);
                    foreach (var element in elements)
                    {
                        element.RemoveAllChildren();
                    }
                }
                var aTags = document.DocumentNode.QuerySelectorAll("a");
                foreach (var aTag in aTags)
                {
                    aTag.Attributes.RemoveAll();
                }


                var articlebody = document.DocumentNode.QuerySelectorAll(".articlebody");
                var contentRaw = "";
                if (articlebody.Any())
                {
                    var storyimages = articlebody.First().QuerySelectorAll(".storyimage");
                    foreach (var storyimage in storyimages)
                    {
                        var jsonString = storyimage.QuerySelector("img").GetAttributeValue("data-src", null);
                        jsonString = jsonString.Replace("&quot;", "\"");

                        var result = from Match match in Regex.Matches(jsonString, "\"([^\"]*)\"")
                                     select match.ToString();

                        string imgSrc = WraperUrl(result.Where(i => i.Contains("//img")).First().Trim('\"'));

                        bool hasCaption = false;

                        if (storyimage.QuerySelectorAll(".caption").Any())
                        {
                            hasCaption = true;
                        }

                        var eCaption = (hasCaption) ? storyimage.QuerySelector(".caption") : null;

                        storyimage.QuerySelector("img").Remove();

                        if (hasCaption)
                        {
                            storyimage.QuerySelector(".caption").Remove();
                        }


                        storyimage.AppendChild(HtmlNode.CreateNode("<img src=\"" + imgSrc + "\"/>"));

                        if (eCaption != null)
                        {
                            storyimage.AppendChild(eCaption);
                        }
                    }

                    contentRaw = articlebody.First().OuterHtml;
                }


                news.contentRaw = contentRaw;

                news.contentHtml = "<!DOCTYPE html>" +
                        "<html>" +
                        "<head>" +
                        "<meta http-equiv=\"Content-Type\" content=\"text/html;charset=utf-8\">" +
                        "</head>" +
                        "<body>" +
                        "<h3 class=\"title\">" + news.title + "</h3>" +
                        "<p>Source: " + news.source + "</p>" +
                        "<p class=\"title\">" + news.contentText + "</p>" +
                        "<div class=\"content-detail\">" + news.contentRaw + "</div>" +
                        "</body>" +
                        "</html>";
                news.contentText = "";

                Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                throw;
            }


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
            else if (input.StartsWith("//static") || input.StartsWith("//img"))
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
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

    }
}
