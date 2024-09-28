using System;
using ii_Yume.Scrappers;
using ii_Yume.Models;
using ii_Yume.Systems;
using System.Diagnostics;
using ii_Yume.DataBase;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Policy;

namespace ii_Yume
{
    internal class Program
    {
        static Novel novelData = new();
        static List<string> chapterData = default;
        static void Main(string[] args)
        {

            //TestNovelScrapper();
            //TestChapterScrapper();
            //TestMainScrapper();
            //TestSearchScrapper();

            Console.ReadKey();
        }

        static async void TestNovelScrapper()
        {
            NovelScrapper novelScrapper = new();
            //novelData = await NovelScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/producto/goblin-slayer-novela-ligera/");


            List<Novel> novels = new();
            StreamReader reader = new(@"D:\carlo\Download\yume\Novel-Links.txt");
            var lines = reader.ReadToEnd().Split('\r');
            string data = string.Empty;
            reader.Close();
            

            for (int i = 0; i < lines.Length-1; i++)
            {
                data = lines[i];
                var novel = await novelScrapper.Init(data);
                //novel.Volumes = await GetParagraphs(novel);
                if (i + 1 == lines.Length)
                {
                    novels.Add(novel);
                    Console.WriteLine($"Fn {i}{lines[i]}");

                    await jsonizer(novel);
                    break;
                }
                Console.WriteLine($"Hola, {i},{lines[i]}");
                novels.Add(novel);

                await jsonizer(novel);
                data = string.Empty;
            }
            Console.WriteLine("\n\nJsonizer Finished");

        }
        static async Task<List<NovelVolume>> GetParagraphs(Novel data)
        {
            ChapterScrapper chapterScrapper = new();
            
            
            List<NovelVolume> result = new();


            foreach (var volume in data.Volumes)
            {
                NovelVolume volumeRes = new();
                volumeRes.VolumNumber = volume.VolumNumber;
                if ((volume != null || data.Volumes.Count != 0) && data.IsFree)
                {
                    foreach (var chapter in volume.Chapters)
                    {
                        Chapter chapterRes = new();
                        chapterRes.ChapterName = chapter.ChapterName;
                        chapterRes.ChapterLink = chapter.ChapterLink;

                        chapterRes.Paragraphs = await chapterScrapper.Init(chapter.ChapterLink);
                        volumeRes.Chapters.Add(chapterRes);
                        
                    }
                }
                result.Add(volumeRes);
            }
            return result;
        }


        static async Task jsonizer(Novel novel)
        {
            string novelName = novel.NovelLink.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[^1].Replace("-novela-web", "").Replace("-novela-ligera", ""); ;
            string filePath = $@"D:\carlo\Download\yumme\{novelName}.json";

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented, // Para que el JSON tenga formato legible
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // Ignorar referencias circulares
            };

            try
            {
                using (StreamWriter streamWriter = new StreamWriter(filePath))
                using (JsonTextWriter jsonWriter = new JsonTextWriter(streamWriter))
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    JsonSerializer serializer = JsonSerializer.Create(settings);
                    serializer.Serialize(jsonWriter, novel);
                }
                Console.WriteLine("JSON saved to file: " + filePath);
            }
            catch (OutOfMemoryException ex)
            {
                Console.WriteLine("OutOfMemoryException: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        




        static async void TestChapterScrapper()
        {
            //chapterData = await ChapterScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/2021/09/30/watashin-nouryoku-volumen-1-capitulo-1-parte-1-novela-ligera/");
            //chapterData = await ChapterScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/2021/09/30/watashin-nouryoku-volumen-1-capitulo-1-parte-1-novela-ligera/");
            //chapterData = await ChapterScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/2023/07/19/sokushi-cheat-volumen-1-capitulo-1-novela-ligera/");
            //chapterData = await ChapterScrapper.Init(@"
            //https://novelasligeras.net/index.php/2021/09/30/watashin-nouryoku-volumen-1-capitulo-1-parte-1-novela-ligera/");
            //chapterData = await ChapterScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/2018/03/19/goblin-slayer-volumen-1-prologo-novela-ligera/");

        }

        static async void TestMainScrapper()
        {
            //var a = MainPage.GetNovels();
        }


    }

}

