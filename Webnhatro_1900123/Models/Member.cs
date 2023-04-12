using System;
using System.Collections.Generic;

namespace Webnhatro_1900123.Models
{
    public partial class Member
    {
        public Member()
        {
            NhaTros = new HashSet<NhaTro>();
        }

        public string Username { get; set; } = null!;
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? HoTen { get; set; }
        public string? Diachi { get; set; }
        public DateTime Ngaythamgia { get; set; }

        public virtual ICollection<NhaTro> NhaTros { get; set; }
    }
}
