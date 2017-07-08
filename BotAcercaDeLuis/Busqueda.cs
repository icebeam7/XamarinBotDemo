using System;
using System.Collections.Generic;
using System.Linq;

using System.Xml.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace BotAcercaDeLuis
{
    public class Busqueda
    {
        static string url = "http://icebeamwp.blogspot.cz/rss.xml";

        public async Task<List<Post>> BuscarPosts(string tag)
        {
            using (var client = new HttpClient())
            {
                var xml = await client.GetStringAsync(url);
                XDocument doc = XDocument.Parse(xml);

                var posts = (from item in doc.Element("rss").Element("channel").Elements("item")
                             select new Post
                             {
                                 Title = item.Element("title").Value,
                                 Link = item.Element("link").Value,
                                 Description = item.Element("description").Value,
                                 PublicationDate = DateTime.Parse(item.Element("pubDate").Value),
                                 GUID = item.Element("guid").Value
                             }).ToList();
                return posts;
            }
        }
    }
}