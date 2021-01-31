using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheDogsCorner.Models
{
    public class KategoriBeskrivning
    {
        public int KategoriId { get; set; }
        [Required(ErrorMessage = "Kategorinamn krävs")]
        [StringLength(100, ErrorMessage = "Minimum 3 och minimum 5 och max 100 tecken är tillåtet", MinimumLength = 3)]
        public string KategoriNamn { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }


}