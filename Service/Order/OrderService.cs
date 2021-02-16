using NRDCL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Models
{
    public class OrderService : IOrderService
    {
        private readonly NRDCL_DB_Context dataBaseContext;

        public OrderService(NRDCL_DB_Context context)
        {
            dataBaseContext = context;
        }
        public List<Order> GetOrderList()
        {
            List<Order> orderList=dataBaseContext.Order_Table.ToList();
            return orderList;
        }
    }
}
