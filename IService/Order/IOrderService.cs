using NRDCL.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Models
{
    public interface IOrderService
    {
        List<Order> GetOrderList();
        ResponseMessage SaveOrder(Order order);
        Order GetOrderDetails(int orderId);
        ResponseMessage UpdateOrder(Order order);
        ResponseMessage DeleteOrder(int orderID);
        bool OrderExists(int orderID);
        decimal CalculateOrderAmount(Order order);
    }
}
