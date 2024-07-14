using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using ii_Yume.Models;
using ii_Yume.DataBase;
using System.Reflection;

namespace ii_Yume.Systems
{
    public class MainPage
    {

        #region Variables

        public static List<Novel> dbData = new();
        static DBNovel dBNovel = new();
        
        #endregion


        #region Unity_Methods

        void Start()
        {

        }

        #endregion


        #region My_Methods
        public static List<Novel> GetNovels()
        {
            return dBNovel.GetNovels();
        }



        #endregion
        

        #region Extras

        public static async Task DownloadImages()
        {
            StreamReader reader = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "Novel-Links.txt"));
            var lines = reader.ReadToEnd();
            string data = string.Empty;
            int index = default;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == '\r')
                {
                    index++;
                    if (i + 1 == lines.Length)
                    {
                        await MainPage.GetImagesFromURL(data);
                        break;
                    }
                    await MainPage.GetImagesFromURL(data);
                    data = string.Empty;
                    if (lines[i + 1] != '\n')
                        continue;

                }
                else if (lines[i] != '\r' || lines[i] != '\n')
                    data += lines[i];
            }
        }
        
        static async Task GetImagesFromURL(string url)
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

            // Encontrar la URL de la imagen
            var imageNode = htmlDoc.DocumentNode.SelectSingleNode("//img[contains(@class, 'wp-post-image')]");


            if (imageNode != null)
            {
                // Extraer el nombre de la novela de la URL y 
                // Remover sufijos "-novela-ligera" y "-novela-web"
                string novelName = url.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[^1].Replace("-novela-web", "").Replace("-novela-ligera", ""); ;

                string imageUrl = imageNode.GetAttributeValue("src", null);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    // Descargar la imagen
                    var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

                    // Guardar la imagen en el dispositivo
                    string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), $"{novelName}.jpg");
                    await File.WriteAllBytesAsync(filePath, imageBytes);

                    Console.WriteLine($"Imagen guardada en: " + filePath);
                }
                else
                {
                    Console.WriteLine("No se encontró la URL de la imagen.");
                }
            }
            else
            {
                Console.WriteLine("No se encontró el nodo de la imagen.");
            }
        }
    
        #endregion

    }

}
