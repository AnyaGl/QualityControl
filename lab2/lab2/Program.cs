using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace lab2
{
    class Program
    {
        static List<string> pages = new List<string>();
        const string MAIN_URL = "http://91.210.252.240/broken-links/";
        static StreamWriter validUrlsFile = new StreamWriter("../../../valid.txt", false, Encoding.UTF8);
        static StreamWriter invalidUrlsFile = new StreamWriter("../../../invalid.txt", false, Encoding.UTF8);
        static int invalidUrlsCounter = 0;
        static HttpClient httpClient = new HttpClient();

        static void Main(string[] args)
        {
            TryToGetLinksFromPage(MAIN_URL);

            WriteToFile(validUrlsFile, $"Links count: {pages.Count - invalidUrlsCounter}\tDate: {DateTime.UtcNow} UTC");
            WriteToFile(invalidUrlsFile, $"Links count: {invalidUrlsCounter}\tDate: {DateTime.UtcNow} UTC");

            validUrlsFile.Close();
            invalidUrlsFile.Close();
        }
        static void TryToGetLinksFromPage(string url)
        {
            try
            {
                GetLinksFromPage(GetLinks(url));
            }
            catch
            {
            }
        }
        static void GetLinksFromPage(List<string> links)
        {
            foreach (var link in links)
            {
                if (!pages.Contains(link))
                {
                    pages.Add(link);
                    string url = link;
                    if (!link.StartsWith("https://") && !link.StartsWith("http://") && !link.StartsWith("ftp://"))
                    {
                        url = MAIN_URL + link;
                        TryToGetLinksFromPage(url);
                    }
                    CheckUrlAndWriteToFile(url);
                }
            }
        }
        static List<string> GetLinks(string url)
        {
            var pageContent = LoadPage(url);
            var document = new HtmlDocument();
            document.LoadHtml(pageContent);

            HtmlNodeCollection hrefs = document.DocumentNode.SelectNodes("//@href");
            HtmlNodeCollection srcs = document.DocumentNode.SelectNodes("//@src");
            HtmlNodeCollection styles = document.DocumentNode.SelectNodes("//@style");

            List<string> links = new List<string>();

            AddLinksFromAttribute(ref links, hrefs, "href");
            AddLinksFromAttribute(ref links, srcs, "src");
            AddUrlsFromStyle(ref links, styles);

            return links;
        }
        static void AddLinksFromAttribute(ref List<string> links, HtmlNodeCollection attributeNodes, string attributeName)
        {
            if (attributeNodes != null)
            {
                foreach (var node in attributeNodes)
                {
                    links.Add(node.GetAttributeValue(attributeName, ""));
                }
            }
        }
        static void AddUrlsFromStyle(ref List<string> links, HtmlNodeCollection styles)
        {
            if (styles != null)
            {
                foreach (var style in styles)
                {
                    var url = Regex.Match(style.GetAttributeValue("style", ""), @"(?<=url\([\']?)(.*)(?=\))").Groups[1].Value;
                    char[] quotes = { '\'', '"' };
                    url = url.Trim(quotes);

                    links.Add(url);
                }

            }
        }
        static void CheckUrlAndWriteToFile(string url)
        {
            using (var response = httpClient.GetAsync(url).Result)
            {
                var message = $"{url}  -  {response.StatusCode.GetHashCode()} {response.ReasonPhrase}";

                if (IsValid(response.StatusCode))
                {
                    WriteToFile(validUrlsFile, message);
                }
                else
                {
                    WriteToFile(invalidUrlsFile, message);
                    invalidUrlsCounter++;
                }
            }
        }
        static bool IsValid(HttpStatusCode statusCode)
        {
            return statusCode.GetHashCode() < 400;
        }

        static void WriteToFile(StreamWriter stream, string message)
        {
            stream.WriteLine(message);
        }
        static string LoadPage(string url)
        {
            var result = "";
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var receiveStream = response.GetResponseStream();
                if (receiveStream != null)
                {
                    StreamReader readStream;
                    if (response.CharacterSet == null)
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    result = readStream.ReadToEnd();
                    readStream.Close();
                }
                response.Close();
            }
            return result;
        }          
    }
}
