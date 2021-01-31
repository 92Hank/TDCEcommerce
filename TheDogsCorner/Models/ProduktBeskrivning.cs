using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheDogsCorner.DAL;

namespace TheDogsCorner.Models
{
    public class ProduktBeskrivning
    {
        public int ProduktId { get; set; }
        [Required(ErrorMessage = "Produktnamn krävs")]
        [StringLength(100, ErrorMessage = "Minimum 3 och minimum 5 och maximum 100 tecken är tillåtet", MinimumLength = 3)]
        public string ProduktNamn { get; set; }
        [Required]
        [Range(1, 50)]
        public Nullable<int> KategoriId { get; set; }
        public Nullable<int> UsersId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> SkapadDatum { get; set; }
        public Nullable<System.DateTime> ModifieradDatum { get; set; }
        [Required(ErrorMessage = "Beskrivning krävs")]
        public string Beskrivning { get; set; }
        public string ProductImage { get; set; }
        public Nullable<bool> IsFeatured { get; set; }
        [Required]
        [Range(typeof(int), "1", "500", ErrorMessage = "Otillåten kvantitet")]
        public Nullable<int> Kvantitet { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "200000", ErrorMessage = "Otillåtet pris")]
        public Nullable<decimal> Pris { get; set; }
        public SelectList Kategorier { get; set; }
    }
}