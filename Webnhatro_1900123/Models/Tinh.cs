using System;
using System.Collections.Generic;

namespace Webnhatro_1900123.Models
{
    public partial class Tinh
    {
        public Tinh()
        {
            KhuVucs = new HashSet<KhuVuc>();
        }

        public string Idtinh { get; set; } = null!;
        public string? TenTinh { get; set; }

        public virtual ICollection<KhuVuc> KhuVucs { get; set; }
    }
}
