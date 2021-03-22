using Microsoft.EntityFrameworkCore;
using NRDCL.Data;
using NRDCL.Models.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Models
{
    public class ProductService : IProductService
    {
        private readonly NRDCL_DB_Context dataBaseContext;
        public CommonProperties CommonProperties { get; set; }

        public ProductService(NRDCL_DB_Context context)
        {
            dataBaseContext = context;
        }

        /// <summary>
        /// Get Product Details
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>product</returns>
        public async Task<Product> GetProductDetails(int productId)
        {
            var product = (from p in dataBaseContext.Product_Table where p.ProductId==productId select p).SingleOrDefault();
            return await Task.FromResult(product);
        }

        /// <summary>
        /// Get Product list
        /// </summary>
        /// <returns>productList</returns>
        public async Task<List<Product>> GetProductList()
        {
            List<Product> productList = dataBaseContext.Product_Table.ToList();
            return await Task.Run(()=> productList);
        }

        /// <summary>
        /// Save Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>responseMessage</returns>
        public async Task<ResponseMessage> SaveProduct(Product product)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            if (product.PricePerUnit <= 0) {
                responseMessage.Status = false;
                responseMessage.Text = CommonProperties.invalidRateMsg;
                responseMessage.MessageKey = "PricePerUnit";
                return await Task.FromResult(responseMessage);
            }

            if (product.TransportRate <= 0)
            {
                responseMessage.Status = false;
                responseMessage.Text = CommonProperties.invalidRateMsg;
                responseMessage.MessageKey = "TransportRate";
                return await Task.FromResult(responseMessage);
            }

            dataBaseContext.Add(product);
            dataBaseContext.SaveChanges();
            responseMessage.Status = true;
            responseMessage.Text = CommonProperties.saveSuccessMsg;

            return await Task.FromResult(responseMessage);
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<ResponseMessage> UpdateProduct(Product product)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                dataBaseContext.Update(product);
                await dataBaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.ProductId))
                {
                    responseMessage.Status = false;
                    responseMessage.Text = CommonProperties.invalidProductMsg;
                    responseMessage.MessageKey = "ProductId";
                    return await Task.FromResult(responseMessage);
                }
                else
                {
                    throw;
                }
            }
            responseMessage.Status = true;
            responseMessage.Text = CommonProperties.updateSuccessMsg;
            return await Task.FromResult(responseMessage);
        }

        /// <summary>
        /// To check product exist or not
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public bool ProductExists(int productID)
        {
            return dataBaseContext.Product_Table.Any(e => e.ProductId == productID);
        }
    }
}
