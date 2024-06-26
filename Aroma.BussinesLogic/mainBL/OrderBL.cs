﻿using Aroma.BussinesLogic.Core.Levels;
using Aroma.BussinesLogic.Interface;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Aroma.BussinesLogic.mainBL
{
    public class OrderBL:UserAPI,IOrderService
    {
        public ResponseAddOrder PurchaseProduct(int userId, int productId, int quantity)
        {
            return purchaseProduct(userId, productId, quantity);
        }

         public ResponseGetOrders ViewOrdersAction(int userId)
        {
            return GetOrders(userId);
        }

        public ResponseGetRating ViewOrdersAction(int userId, int productId, int rating, string review)
        {
            return GetRating(userId, productId, rating, review );
        }

        public ResponseGetOrders ViewOrdersUserAction(int userId)
        {
            return GetUserOrders(userId);
        }

       public  ResponseUpdateQuantityOrders EditQuntity(int userId, int productId, int quantityOrder)
        {
            return EditOrderQuantity(userId, productId, quantityOrder);
        }
        public async Task<ResponseGetOrders> DeleteOrderAction(int userId, int productId)
        {
            return await  DeleteOrder(userId, productId);
        }
        public async Task<ResponseGetOrders> ConfirmPurchaseUserAction(int userId)
        {
            return await ConfirmPurchaseAction(userId);
        }

        public async Task<ResponseGetProducts> SearchProducts(string searchTerm)
        {
            return await SearchProductsAction(searchTerm);
        }
        public ResponseViewProduct ViewProductInfo(int userId, int productId)
        {
            return ViewProductInfoAction(userId, productId);
        }

 
        public ResponseGetOrders AddFeedback(int productId,int userId)
        {
            return AddFeedbackAction(productId,userId);
        }

    }
}
