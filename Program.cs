using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JsonPractice
{
    class Program
    {
        private static async Task<string> GetJson()
        {
            var client = new HttpClient();
            var result = await client.GetAsync("https://newsapi.org/v2/top-headlines?country=us&apiKey=7b14f60aa1334a61912771ea556ae7e4");
            return await result.Content.ReadAsStringAsync();
        }
        static async Task Main(string[] args)
        {
            var news = await GetJson();
            News newsObject = JsonConvert.DeserializeObject<News>(news);
            var filtered = newsObject.articles.Where(x => x.author != null && x.author.ToLower().StartsWith("a"));
            foreach (var item in filtered)
            {
                Console.WriteLine(item.author);
            }
            var pictures = newsObject.articles.Where(p => p.urlToImage != null);
            var imgName = 0;
            foreach (var pic in pictures)
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFileAsync(new Uri(pic.urlToImage.ToString()), (imgName++.ToString()+".jpg"));
                }
            }
            Console.ReadLine();
        }

        public class News
        {
            public string status { get; set; }
            public int totalResults { get; set; }
            public List<Article> articles { get; set; }
        }
        public class Article
        {
            public Source source { get; set; }
            public string author { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public string url { get; set; }
            public string urlToImage { get; set; }
            public string publishedAt { get; set; }
            public string content { get; set; }

        }
        public class Source
        {
            public string id { get; set; }
            public string name { get; set; }
        }
    }
}
