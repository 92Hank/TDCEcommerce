using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheDogsCorner.DAL;

namespace TheDogsCorner.Models.Home
{
    public class Item
    {
        public Produkt Produkt { get; set; }
        public int Kvantitet { get; set; }

        public Item()
        {

        }
        public Item(Produkt produkt, int kvantitet)
        {
            Produkt = produkt;
            Kvantitet = kvantitet;
        }
    }
}