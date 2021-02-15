using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NRDCL.Models
{
    public class ProductBusinessLayer
    {
        //public NRDCL_DB_Entities dbContext = new NRDCL_DB_Entities();

        public List<Product> GetProductList()
        {
            List<Product> productList = new List<Product>();
            Product product = new Product();
            product.ProductId = 1;
            product.ProductName = "Product One";
            product.TransportRate = decimal.Zero;
            productList.Add(product);
            //List<Product> productList = dbContext.Products.ToList();
            return productList;
        }
        public Product GetProductDetail(int ProductId)
        {
            Product product = new Product();
            product.ProductId = 1;
            product.ProductName = "Product One";
            product.TransportRate = decimal.Zero;
           // var product = (from p in dbContext.Products where p.ProductId == ProductId select p).FirstOrDefault();
            return product;
        }
    }
}