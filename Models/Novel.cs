using System;
using System.Collections.Generic;
using System.Collections;

namespace ii_Yume.Models
{
    public class Novel{
        public int ID { get; set; }
        public string? NovelName { get; set; }
        public string? NovelLink { get; set; }
        public string? NovelGenders { get; set;}
        public float? Stars { get; set; }
        public string? Synopsis { get; set; }
        public List<NovelVolume>? Volumes { get; set; } = new();
        public bool IsFree { get; set; }
    }

}
