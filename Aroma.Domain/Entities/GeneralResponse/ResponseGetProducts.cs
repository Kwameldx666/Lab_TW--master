﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aroma.Domain.Entities.Product;
using Aroma.Domain.Entities.Product.DBModel;

namespace Aroma.Domain.Entities.GeneralResponse
{
    public class ResponseGetProducts
    {
        public bool Status { get; set; } // Успех операции
        public string Message { get; set; } // Сообщение, например, об ошибке

        public List<Product.DBModel.Product> Products;

        private List<ProductDbTable> searchResults;

        public List<Product.DBModel.Product> ProductsV2;

        public List<Product.DBModel.Product> BestSellers;



    }

}
