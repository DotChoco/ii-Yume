using ii_Yume.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ii_Yume.DataBase
{
    public class DBNovel
    {

        #region Variables

        List<Novel> data = new();
        string dbFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string folderName = "ii_Yume";
        string dbName = "Data.db";
        string dbFullPath = string.Empty;

        #endregion

        public void TestQuery()
        {
            //GetAllData();
            //SaveAnItem(new()
            //{
            //    Name = "LoveNPeace",
            //    InventoryPos = "4:12",
            //    Rarity = Rarity.Legendary,
            //    Amount = 1,
            //    Price = 200000,
            //    Type = IOType.Angelical,
            //    Consumable = true,
            //    Description = "It's an Custom Angelical Item"
            //});
        }



        #region My_Methods


        public List<Novel> GetNovels()
        {
            return data;
        }


        void CreateTable()
        {
            CreateDirectory();
        }


        void CreateDirectory()
        {
            dbFullPath = $@"{dbFolderPath}\{folderName}\{dbName}";
            if (!Directory.Exists(dbFolderPath))
                Directory.CreateDirectory(dbFolderPath);
        }


        #endregion

    }
}
