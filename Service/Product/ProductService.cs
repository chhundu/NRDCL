using Microsoft.EntityFrameworkCore;
using NRDCL.Data;
using NRDCL.Models.Common;
using System;
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
        public Product GetProductDetails(int productId)
        {
            var product = (from p in dataBaseContext.Product_Table where p.ProductId==productId select p).SingleOrDefault();
            return product;
        }

        /// <summary>
        /// Get Product list
        /// </summary>
        /// <returns>productList</returns>
        public List<Product> GetProductList()
        {
            List<Product> productList = dataBaseContext.Product_Table.ToList();
            return productList;
        }

        /// <summary>
        /// Save Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>responseMessage</returns>
        public ResponseMessage SaveProduct(Product product)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            if (product.PricePerUnit <= 0) {
                responseMessage.Status = false;
                responseMessage.Text = CommonProperties.invalidRateMsg;
                responseMessage.MessageKey = "PricePerUnit";
                return responseMessage;
            }

            if (product.TransportRate <= 0)
            {
                responseMessage.Status = false;
                responseMessage.Text = CommonProperties.invalidRateMsg;
                responseMessage.MessageKey = "TransportRate";
                return responseMessage;
            }

            dataBaseContext.Add(product);
            dataBaseContext.SaveChanges();
            responseMessage.Status = true;
            responseMessage.Text = CommonProperties.saveSuccessMsg;

            return responseMessage;
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public ResponseMessage UpdateProduct(Product product)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                dataBaseContext.Update(product);
                dataBaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.ProductId))
                {
                    responseMessage.Status = false;
                    responseMessage.Text = CommonProperties.invalidProductMsg;
                    responseMessage.MessageKey = "ProductId";
                    return responseMessage;
                }
                else
                {
                    throw;
                }
            }
            responseMessage.Status = true;
            responseMessage.Text = CommonProperties.updateSuccessMsg;
            return responseMessage;
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
