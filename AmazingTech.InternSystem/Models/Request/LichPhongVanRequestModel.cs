﻿using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace AmazingTech.InternSystem.Models.Request
{
    public class LichPhongVanRequestModel
    {
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ThoiGianPhongVan { get; set; }
        public InterviewForm interviewForm { get; set; }
        public int TimeDuration { get; set; }
        public string HoVaTenNgPhongVan { get;set; }
        public string DiaDiemPhongVan { get; set; }
    }
}
