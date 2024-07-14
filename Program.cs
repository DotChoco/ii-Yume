using System;
using ii_Yume.Scrappers;
using ii_Yume.Models;
using ii_Yume.Systems;
using System.Diagnostics;
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
            TestMainScrapper();
            //TestSearchScrapper();

            Console.ReadKey();
        }

        static async void TestNovelScrapper()
        {
            //novelData = await NovelScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/producto/goblin-slayer-novela-ligera/");
            //novelData = await NovelScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/producto/sokushi-cheat-ga-saikyou-novela-ligera/");
            //novelData = await NovelScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/producto/saijaku-muhai-no-bahamut-novela-ligera/");
            //novelData = await NovelScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/producto/trpg-player-ga-isekai-de-saikyou-build-wo-mezasu-novela-ligera/");
            //novelData = await NovelScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/producto/watashi-nouryoku-TestNovelScrapper-heikinchi-dette-itta-yo-ne-novela-ligera/");
            //novelData = await NovelScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/producto/toradora-novela-ligera/");
            //novelData = await NovelScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/producto/shinmai-maou-no-keiyakusha-novela-ligera/");
            //novelData = await NovelScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/producto/shinja-zero-no-megami-sama-novela-ligera/");
            //novelData = await NovelScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/producto/atelier-tanaka-novela-ligera/");
            novelData = await NovelScrapper.Init(@"
            https://novelasligeras.net/index.php/producto/kage-no-jitsuryokusha-ni-naritakute-novela-ligera/");

        }


        static async void TestChapterScrapper()
        {
            //chapterData = await ChapterScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/2021/09/30/watashin-nouryoku-volumen-1-capitulo-1-parte-1-novela-ligera/");
            //chapterData = await ChapterScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/2021/09/30/watashin-nouryoku-volumen-1-capitulo-1-parte-1-novela-ligera/");
            //chapterData = await ChapterScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/2023/07/19/sokushi-cheat-volumen-1-capitulo-1-novela-ligera/");
            chapterData = await ChapterScrapper.Init(@"
            https://novelasligeras.net/index.php/2021/09/30/watashin-nouryoku-volumen-1-capitulo-1-parte-1-novela-ligera/");
            //chapterData = await ChapterScrapper.GetNovels(@"
            //https://novelasligeras.net/index.php/2018/03/19/goblin-slayer-volumen-1-prologo-novela-ligera/");

        }

        static async void TestMainScrapper()
        {
            await MainPage.GetNovels();
        }

        static async void TestSearchScrapper()
        {
            SearchNovels.SearchByName("Goblin");
        }



    }

}

