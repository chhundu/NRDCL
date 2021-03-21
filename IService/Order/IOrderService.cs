using NRDCL.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Models
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrderList();
        ResponseMessage SaveOrder(Order order);
        Task<Order> GetOrderDetails(int orderId);
        ResponseMessage UpdateOrder(Order order);
        Task<ResponseMessage> DeleteOrder(int orderID);
        bool OrderExists(int orderID);
        Task<decimal> CalculateOrderAmount(Order order);
    }
}
