using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ii_Yume.Models
{
    public class Chapter
    {
        public string? ChapterName { get; set; }
        public string? ChapterLink { get; set; }
        public bool? ChapterRead { get; set; } = false;
        public List<string>? Paragraphs { get; set; } = new();
    }
}
