using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ii_Yume.Models
{
    public class NovelVolume
    {
        public string? VolumNumber { get; set; }
        public List<Chapter>? Chapters { get; set; } = new();
    }
}
