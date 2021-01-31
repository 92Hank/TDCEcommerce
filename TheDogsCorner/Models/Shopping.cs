using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheDogsCorner.Models
{
    public class Fraktdetalj
    {
        public int FraktDetaljId { get; set; }
        [Required]
        public Nullable<int> MedlemId { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public string Stad { get; set; }
        public string Stat { get; set; }
        [Required]
        public string Land { get; set; }
        [Required]
        public string Postnummer { get; set; }
        public Nullable<int> OrderId { get; set; }
        public Nullable<decimal> BetaltBelopp { get; set; }
        [Required]
        public string BetalTyp { get; set; }
    }
}