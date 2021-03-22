using NRDCL.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NRDCL.Models
{
    public interface IProductService
    {
        Task<List<Product>> GetProductList();
        Task<Product> GetProductDetails(int productId);
        Task<ResponseMessage> SaveProduct(Product product);
        Task<ResponseMessage> UpdateProduct(Product product);
        bool ProductExists(int productId);
    }
}
