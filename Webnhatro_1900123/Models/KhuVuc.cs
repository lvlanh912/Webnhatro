using System;
using System.Collections.Generic;

namespace Webnhatro_1900123.Models
{
    public partial class KhuVuc
    {
        public KhuVuc()
        {
            Xas = new HashSet<Xa>();
        }

        public string IdkhuVuc { get; set; } = null!;
        public string? TenKhuVuc { get; set; }
        public string? Idtinh { get; set; }

        public virtual Tinh? IdtinhNavigation { get; set; }
        public virtual ICollection<Xa> Xas { get; set; }
    }
}
