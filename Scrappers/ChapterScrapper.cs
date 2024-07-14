using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace ii_Yume.Scrappers
{
    public class ChapterScrapper
    {
        public static List<string> Paragraphs = new();
        public static async Task<List<string>> Init(string url)
        {
            // Crear HttpClientHandler para configurar las opciones del cliente
            var httpClientHandler = new HttpClientHandler();

            // Crear HttpClient y establecer el encabezado User-Agent
            var httpClient = new HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            // Descargar el HTML
            var html = await httpClient.GetStringAsync(url);

            // Cargar el HTML en HtmlAgilityPack
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            return GetParagraphs(htmlDoc);
        }

        static List<string> GetParagraphs(HtmlDocument htmlDoc)
        {
            var paragraphNodes = htmlDoc.DocumentNode.SelectNodes("//p");
            string data = default;

            for (int i = 0; i < paragraphNodes.Count; i++)
            {
                if (RemoveToList(paragraphNodes[i], i))
                {
                    paragraphNodes.RemoveAt(i);
                    i = -1;
                    continue;
                }
                else
                {
                    paragraphNodes[i].InnerText
                                .Replace("&#8230;", "...")
                                .Replace("Sinopsis por NOVA", "");
                }
            }

            for (int i = 0; i < paragraphNodes.Count; i++)
            {
                data = paragraphNodes[i].InnerText;
                if (data == @"\n" || data == string.Empty || data == "\n")
                {
                    paragraphNodes.Remove(paragraphNodes[i]);
                    i = -1;
                    continue;
                }
            }

            foreach (var node in paragraphNodes)
            {
                if (node.InnerText.Contains("Visita la página web del traductor"))
                    break;
                else
                {
                    string dataNode = node.InnerText
                            .Replace("&#8230;", "...")
                            .Replace("&#8220;", "\"")
                            .Replace("&#8221;", "\"");

                    Paragraphs.Add(dataNode);
                    Console.WriteLine(dataNode);
                }
            }

            return Paragraphs;
        }

        static bool RemoveToList(HtmlNode node, int index)
        {
            if(index == 125)
                Console.WriteLine();
            if ( node.InnerText == "&nbsp;" || node.InnerText == "***")
            {
                return true;
            }
            
            if((node.InnerHtml == string.Empty && node.InnerText == string.Empty) 
                || node.InnerHtml.Contains("¡Encuentra todas tus novelas ligeras favoritas aqui!")
                || node.FirstChild.Name == "img")
            {
                return true;
            }
            return false;
        }


    }
}
