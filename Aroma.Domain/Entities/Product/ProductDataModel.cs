﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.Product
{
    public class ProductDataModel
    {
        public DBModel.Product AddSingleProduct { get; set; }
        public List<DBModel.Product> AddProducts { get; set; }
    }
}
