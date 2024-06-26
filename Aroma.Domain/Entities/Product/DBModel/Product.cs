﻿using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Enums.OrdersStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.Product.DBModel
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string ProductType { get; set; }

        public decimal TotalAmount { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public OrderStatus orderStatus { get; set; }
        public string Feedback { get; set; }

        public int Reting { get; set; }

        public double AverageRating { get;set; }

        public int Discount { get; set; }

        public int View { get; set; }

        public decimal PriceWithDiscount { get; set; }
    }
}
