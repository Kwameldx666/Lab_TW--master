﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Lab_TW.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ProductType { get; set; }
        public string Category {get; set; }

        public string ImageUrl { get; set; }

        public int Quantity { get; set; }

        public string Brand {  get; set; }
        public int Rating { get;set; }

        public double AverageReting { get; set; }
        public string Feedback { get; set; }
        
        public decimal Discount { get; set; }

        public int View { get; set;}

        public int Code { get; set; }

        public decimal PriceWithDiscount { get; set; }
    }
}