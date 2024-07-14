using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using ii_Yume.Models;

namespace ii_Yume.Systems
{
    public class SearchNovels
    {
        public static List<Novel> GetNovels(string novelName)
        {
            return SearchByName(novelName);
        }

        static List<Novel> SearchByName(string novelName)
        {
            List<Novel> result = new();
            MainPage.dbData.ForEach(novel => {
                if (novel.NovelName.ToLower() == novelName.ToLower()
                || novel.NovelName.ToLower().Contains(novelName.ToLower()))
                {
                    result.Add(novel);
                }
            });
            return result;
        }

    }

}

