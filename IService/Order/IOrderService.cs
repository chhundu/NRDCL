using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Models
{
    public interface IOrderService
    {
        List<Order> GetOrderList();
    }
}
