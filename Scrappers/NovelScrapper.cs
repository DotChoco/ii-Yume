using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using ii_Yume.Models;


namespace ii_Yume.Scrappers
{
    public class NovelScrapper
    {

        public static Novel NovelData = new();
        public static async Task<Novel> Init(string url)
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


            // Extraer la sinopsis
            GetSynopsis(htmlDoc);

            // Extraer el nombre
            GetName(htmlDoc);

            // Agrega el link de la novela
            SetLink(url);

            // Extraer la lista de capítulos
            GetGenders(htmlDoc);
            
            // Extraer la lista de capítulos
            GetStars(htmlDoc);

            // Revisa que no sea una novela exclusiva
            if (IsFree(htmlDoc))
            {
                // Extraer la lista de capítulos
                GetChapters(htmlDoc);
            }

            // Mostrar la información
            PrintData(NovelData);

            return NovelData;
        }


        #region GetData

        static bool IsFree(HtmlDocument htmlDoc)
        {
            var freeNovel = htmlDoc.DocumentNode.SelectNodes("//span[@class='berocket_tooltip_text']");
            if (freeNovel != null)
            {
                foreach (var node in freeNovel)
                {
                    if (node.InnerText.Contains("Semi Liberada") ||
                        node.InnerText.Contains("No Liberada"))
                    {
                        NovelData.IsFree = true;
                    }
                    else
                    {
                        NovelData.IsFree = false;
                    }
                }
            }
            return NovelData.IsFree;
        }

        static void SetLink(string url) => NovelData.NovelLink = url;

        static void GetName(HtmlDocument htmlDoc)
        {
            var nameNode = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='product_title entry-title']");
            NovelScrapper.NovelData.NovelName = nameNode?.InnerText.Trim();
        }

        static void GetSynopsis(HtmlDocument htmlDoc)
        {
            var synopsisNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='woocommerce-product-details__short-description']");
            NovelData.Synopsis = synopsisNode?.InnerText.Trim()
                                .Replace("&#8230;", "...")
                                .Replace("&#8220;", "\"")
                                .Replace("&#8221;", "\"")
                                .Replace("Sinopsis por NOVA", "");
            string synopsis = NovelData.Synopsis;
            NovelData.Synopsis = default;
            for (int i = 0; i < synopsis.Length; i++)
            {
                if (i+1 < synopsis.Length)
                {
                    if (synopsis[i] == '&' && synopsis[i+5] == ';')
                    {
                        break;
                    }
                }
                NovelData.Synopsis += synopsis[i];
            }
        }

        static void GetGenders(HtmlDocument htmlDoc)
        {
            var synopsisNode = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='posted_in']");
            NovelScrapper.NovelData.NovelGenders = synopsisNode?.InnerText.Trim().Remove(0, 12);
        }
        
        static void GetStars(HtmlDocument htmlDoc)
        {
            var synopsisNode = htmlDoc.DocumentNode.SelectSingleNode("//strong[@class='rating']");
            if (synopsisNode != null)
                NovelScrapper.NovelData.Stars = float.Parse(synopsisNode?.InnerText.Trim());
            else
                NovelScrapper.NovelData.Stars = 0;
        }

        static void GetChapters(HtmlDocument htmlDoc)
        {
            List<string> chapters = new List<string>();
            List<string> chaptersLinks = new List<string>();
            List<Chapter> finalCaps = new();
            int counter = default;


            var chaptersNode = htmlDoc.DocumentNode.SelectNodes("//a");

            // Definir la expresión regular para la URL específica
            var regex = new Regex(@"https://novelasligeras\.net/index\.php/\d+/\d+/\d+/.+");

            // Obtiene los capitulos junto a sus enlaces
            foreach (var chapterNode in chaptersNode)
            {
                var href = chapterNode.GetAttributeValue("href", "");
                if (regex.IsMatch(href))
                {
                    chapters.Add(chapterNode.InnerText
                        .Replace("&nbsp;", "")
                        .Replace("&#8230;", "...")
                        .Replace("&#8220;", "\"")
                        .Replace("&#8221;", "\""));
                    chaptersLinks.Add(href);
                }
            }
                                                                                                                                                                                                                                                                                                                                          

            #region AttachChaptersToVolumes 

            var VolumeNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'wpb_wrapper')]");

            for (int i = 0; i < VolumeNodes.Count; i++)
            {
                if(VolumeNodes[i].InnerText == string.Empty)
                {
                    VolumeNodes.Remove(VolumeNodes[i]);
                    i = -1;
                }
                else if (!VolumeNodes[i].InnerText.Contains("Volumen") 
                    || VolumeNodes[i].InnerText.Length <=9 || VolumeNodes[i].InnerText.First() != 'V')
                {
                    VolumeNodes.Remove(VolumeNodes[i]);
                    i = -1;
                }
            }

            foreach (var Volume in VolumeNodes)
            {
                while (counter <= chapters.Count-1 && 
                    Volume.InnerText.Contains(chapters[counter]))
                {
                    finalCaps.Add(new()
                    {
                        ChapterName = chapters[counter],
                        ChapterLink = chaptersLinks[counter],
                    });
                    counter++;
                }
                if(counter >= chapters.Count)
                {
                    if (counter > chapters.Count - 1)
                    {
                        if (Volume.InnerText.Contains(chapters[counter-1]))
                        {
                            foreach (var item in NovelData.Volumes)
                            {
                                if (item.Chapters == finalCaps)
                                    break;
                            }
                            AddChaptersToVolume(VolumeNodes.IndexOf(Volume) + 1, finalCaps);
                        }
                        break;
                    } 
                    else if(Volume.InnerText.Contains(chapters[counter]))
                    {
                        AddChaptersToVolume(VolumeNodes.IndexOf(Volume) + 1, finalCaps);
                    }
                }
                else
                {
                    AddChaptersToVolume(VolumeNodes.IndexOf(Volume) + 1, finalCaps);
                    finalCaps = new();
                }
            }

            #endregion

        }

        #endregion


        static void AddChaptersToVolume(int volumeNum, List<Chapter> caps)
        {
            NovelData.Volumes.Add(new()
            {
                VolumNumber = $"Volumen {volumeNum}",
                Chapters = caps
            });
        }

        static void PrintData(Novel novel)
        {
            Console.WriteLine("Nombre: " + novel.NovelName);
            Console.WriteLine("\n");

            Console.WriteLine("Stars: " + novel.Stars);
            Console.WriteLine("\n");

            Console.WriteLine("Generos: " + novel.NovelGenders);
            Console.WriteLine("\n");

            Console.WriteLine("Sinopsis: " + novel.Synopsis);
            Console.WriteLine("\n");

            Console.WriteLine("Capítulos:");
            foreach (var volume in novel.Volumes)
            {
                Console.WriteLine(volume.VolumNumber);
                foreach (var chapter in volume.Chapters)
                {
                    Console.WriteLine("\t- " + chapter.ChapterName);
                    Console.WriteLine("\t\t*- " + chapter.ChapterLink);
                }
            }
        }
    
        
    }
}
