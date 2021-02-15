using NRDCL.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Models
{
    public interface IProductService
    {
        List<Product> GetProductList();
        Product GetProductDetails(int productId);
        ResponseMessage SaveProduct(Product product);
        ResponseMessage UpdateProduct(Product product);
        bool ProductExists(int productId);
    }
}
