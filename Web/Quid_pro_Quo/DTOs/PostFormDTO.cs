﻿using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;

namespace Quid_pro_Quo.DTOs
{
    public class PostFormDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public IEnumerable<IFormFile> ImageFiles { get; set; }
        public string AuthorName { get; set; }
        public DateTime PostedAt { get; set; }
        public bool IsActual { get; set; }

        public IEnumerable<DateTime> PerformServiceOnDatesList { get; set; }

        public double PerformServiceInPlaceLat { get; set; }
        public double PerformServiceInPlaceLng { get; set; }
        public double PerformServiceInPlaceZoom { get; set; }
    }
}
