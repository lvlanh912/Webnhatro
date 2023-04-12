using System;
using System.Collections.Generic;

namespace Webnhatro_1900123.Models
{
    public partial class TinTuc
    {
        public string Idnews { get; set; } = null!;
        public string? Title { get; set; }
        public string? NoiDung { get; set; }
        public string? Anh { get; set; }
        public DateTime NgayDang { get; set; }
    }
}
