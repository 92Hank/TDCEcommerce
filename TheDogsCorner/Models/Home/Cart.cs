using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheDogsCorner.DAL;

namespace TheDogsCorner.Models.Home
{
    public class Cart
    {
        public Produkt Produkt { get; set; }
        public int Quantity { get; set; }

        public Cart (Produkt produkt, int quantity)
        {
            Produkt = produkt;
            Quantity = quantity;
        }
    }
}