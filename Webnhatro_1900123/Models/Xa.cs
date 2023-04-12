using System;
using System.Collections.Generic;

namespace Webnhatro_1900123.Models
{
    public partial class Xa
    {
        public Xa()
        {
            NhaTros = new HashSet<NhaTro>();
        }

        public string Idxa { get; set; } = null!;
        public string? TenXa { get; set; }
        public string? IdkhuVuc { get; set; }

        public virtual KhuVuc? IdkhuVucNavigation { get; set; }
        public virtual ICollection<NhaTro> NhaTros { get; set; }
    }
}
