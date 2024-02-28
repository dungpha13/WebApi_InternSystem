using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazingTech.InternSystem.Models.Response
{
    public class TruongResponseDTO
    {
        public string Id { get; set; }
        public string? Ten { get; set; }
        public int SoTuanThucTap { get; set; }
        public int Interns { get; set; }
    }
}