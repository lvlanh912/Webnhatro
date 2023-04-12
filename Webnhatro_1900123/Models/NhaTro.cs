using System;
using System.Collections.Generic;

namespace Webnhatro_1900123.Models
{
    public partial class NhaTro
    {
        public string Idtro { get; set; } = null!;
        public string? TieuDe { get; set; }
        public int? Dientich { get; set; }
        public int Gia { get; set; }
        public string? Thongtin { get; set; }
        public string? Anh { get; set; }
        public DateTime Thoigiandang { get; set; }
        public string? Idxa { get; set; }
        public string? Username { get; set; }

        public virtual Xa? IdxaNavigation { get; set; }
        public virtual Member? UsernameNavigation { get; set; }
    }
}
