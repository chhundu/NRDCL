using Microsoft.EntityFrameworkCore;
using NRDCL.Data;
using NRDCL.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Models
{
    public class OrderService : IOrderService
    {
        private readonly NRDCL_DB_Context dataBaseContext;
        private readonly ICustomerService customerService;
        private readonly IDepositService depositService;
        public CommonProperties CommonProperties { get; set; }

        public OrderService(NRDCL_DB_Context context, ICustomerService customer, IDepositService deposit)
        {
            dataBaseContext = context;
            customerService = customer;
            depositService = deposit;
        }
        public List<Order> GetOrderList()
        {
            List<Order> orderList = (from ord in dataBaseContext.Order_Table
                                     join cus in dataBaseContext.Customer_Table on ord.CustomerID equals cus.CitizenshipID
                                     join site in dataBaseContext.Site_Table on ord.SiteID equals site.SiteId
                                     join p in dataBaseContext.Product_Table on ord.ProductID equals p.ProductId
                                     join d in dataBaseContext.Deposit_Table on ord.CustomerID equals d.CustomerID
                                     select new Order
                                     {
                                         OrderID=ord.OrderID,
                                         CustomerID=ord.CustomerID,
                                         CustomerName=cus.CustomerName,
                                         SiteID=ord.SiteID,
                                         SiteName=site.SiteName,
                                         ProductID=ord.ProductID,
                                         ProductName=p.ProductName,
                                         Quantity=ord.Quantity,
                                         OrderAmount=ord.OrderAmount,
                                         Balance = d.Balance
                                     }).ToList();
            return orderList;
        }

        /// <summary>
        /// To save order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public ResponseMessage SaveOrder(Order order)
        {
            var responseMessage = new ResponseMessage();

            /// check whether there is data or not for dropdown.
            var checkEmptyResponseMessage = CheckEmptyData();
            if (!checkEmptyResponseMessage.Status)
            {
                responseMessage.Status = false;
                responseMessage.Text = checkEmptyResponseMessage.Text;
                responseMessage.MessageKey = "NoData";
                return responseMessage;
            }

            if (!customerService.IsCustomerExist(order.CustomerID)) {
                responseMessage.Status = false;
                responseMessage.Text = CommonProperties.citizenshipIDNotRegisteredMsg;
                responseMessage.MessageKey = "CustomerID";
                return responseMessage;
            }

            ///validating order amount
            decimal orderAmount = CalculateOrderAmount(order);
            if (orderAmount != order.OrderAmount)
            {
                responseMessage.Status = false;
                responseMessage.Text = CommonProperties.wrongOrderAmountMsg;
                responseMessage.MessageKey = "OrderAmount";
                return responseMessage;
            }

            ///checking available customer balance to place order
            decimal advanceBalance = decimal.Zero;
            /// getting customer balance
            advanceBalance = (from deposit in dataBaseContext.Deposit_Table
                              where deposit.CustomerID.Equals(order.CustomerID)
                              select deposit.Balance).FirstOrDefault();
            if (advanceBalance < orderAmount)
            {
                /// amount that is needed to place an order.
                decimal additionalAmountRequired = orderAmount - advanceBalance;
                responseMessage.Status = false;
                responseMessage.Text = Math.Abs(additionalAmountRequired) + " additional amount is required to place the order.";
                responseMessage.MessageKey = "OrderAmount";
                return responseMessage;
            }

            /// convert order data for saving
            Order validOrderData = ConvertForSaving(order);
            if (!validOrderData.Status)
            {
                responseMessage.Status = validOrderData.Status;
                responseMessage.Text = validOrderData.Message;
                responseMessage.MessageKey = "OrderAmount";
                return responseMessage;
            }

            /// saving and updating amount in deposite table
            var deposits = new Deposit();
            deposits.CustomerID = validOrderData.CustomerID;
            deposits.LastAmount = (from deposit in dataBaseContext.Deposit_Table
                                   where deposit.CustomerID.Equals(validOrderData.CustomerID)
                                   select deposit.LastAmount).FirstOrDefault();

            deposits.Balance = (from deposit in dataBaseContext.Deposit_Table
                                where deposit.CustomerID.Equals(validOrderData.CustomerID)
                                select deposit.Balance).FirstOrDefault() - (validOrderData.OrderAmount);

            depositService.DeleteDeposit(validOrderData.CustomerID);
            depositService.SaveDeposit(deposits);

            /// saving/updating order
            dataBaseContext.Add(validOrderData);
            dataBaseContext.SaveChanges();
            responseMessage.Status = true;
            responseMessage.Text = validOrderData.Message;
            return responseMessage;
        }

        /// <summary>
        /// To get order detail
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Order GetOrderDetails(int orderId)
        {
            var order = (from ord in dataBaseContext.Order_Table 
                         join c in dataBaseContext.Customer_Table on ord.CustomerID equals c.CitizenshipID
                         join site in dataBaseContext.Site_Table on ord.SiteID equals site.SiteId
                         join p in dataBaseContext.Product_Table on ord.ProductID equals p.ProductId
                         join d in dataBaseContext.Deposit_Table on ord.CustomerID equals d.CustomerID
                         where ord.OrderID== orderId 
                         select new Order {
                             OrderID=ord.OrderID,
                             CustomerID=ord.CustomerID,
                             CustomerName=c.CustomerName,
                             SiteID=ord.SiteID,
                             SiteName=site.SiteName,
                             ProductID=ord.ProductID,
                             ProductName=p.ProductName,
                             Quantity=ord.Quantity,
                             OrderAmount =ord.OrderAmount,
                             Balance=d.Balance
                         }).SingleOrDefault();
            return order;
        }

        /// <summary>
        /// Update order inofrmation
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public ResponseMessage UpdateOrder(Order order)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            if (GetOrderDetails(order.OrderID)!=null){
                dataBaseContext.Update(order);
                dataBaseContext.SaveChangesAsync();
                responseMessage.Status = true;
                responseMessage.Text = CommonProperties.updateSuccessMsg;
            }
               
            return responseMessage;
        }
        /// <summary>
        /// Delete order information
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public ResponseMessage DeleteOrder(int orderID)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            Order orderDetail = GetOrderDetails(orderID);
            Deposit depositDetail = depositService.GetDepositDetails(orderDetail.CustomerID);

            decimal lastAmount = orderDetail.OrderAmount;
            decimal balanace = (depositDetail.Balance) + (orderDetail.OrderAmount);

            Deposit deposit = new Deposit();
            deposit.CustomerID=orderDetail.CustomerID;
            deposit.LastAmount= lastAmount;
            deposit.Balance= balanace;

            depositService.DeleteDeposit(orderDetail.CustomerID);
            depositService.SaveDeposit(deposit);
           
            dataBaseContext.Order_Table.Remove(orderDetail);
            dataBaseContext.SaveChanges();
            responseMessage.Status = true;
            responseMessage.Text = CommonProperties.deleteSuccessMsg;
            return responseMessage;
        }

        public bool OrderExists(int orderID)
        {
            return dataBaseContext.Order_Table.Any(e => e.OrderID == orderID);
        }

        /// <summary>
        /// Calcuklate orderamount
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public decimal CalculateOrderAmount(Order order)
        {
            var orders = new Order();
            decimal orderAmount = decimal.Zero;
            decimal advanceBalance = decimal.Zero;
            decimal unitPrice = decimal.Zero;
            decimal transportRate = decimal.Zero;
            double distanceFrom = 0.0;

            /// inputs value
            var customerID = order.CustomerID;
            var siteID = order.SiteID;
            var productID = order.ProductID;
            var quantity = order.Quantity;

            /// getting product amount based on product ID.
            var product = (from p in dataBaseContext.Product_Table
                           where p.ProductId == productID
                           select new Product
                           {
                               PricePerUnit = p.PricePerUnit,
                               TransportRate = p.TransportRate
                           }).FirstOrDefault();
            unitPrice = product.PricePerUnit;
            transportRate = product.TransportRate;

            /// Getting distance from based on sideID
            distanceFrom = (from site in dataBaseContext.Site_Table
                            where site.CitizenshipID.Equals(customerID)
                            select site.DistanceFrom).FirstOrDefault();

            /// getting customer balance
            advanceBalance = (from d in dataBaseContext.Deposit_Table
                              where d.CustomerID.Equals(customerID)
                              select d.Balance).FirstOrDefault();

            /// amount customer need to pay.
            orderAmount = (unitPrice * quantity) + (transportRate * quantity * (decimal)distanceFrom);

            return orderAmount;
        }

        #region private methods
        /// <summary>
        /// this method check whether there is data or not.
        /// </summary>
        /// <returns></returns>
        private ResponseMessage CheckEmptyData()
        {
            var responseMessage = new ResponseMessage();

            /// check whether there is customer data or not
            if (!dataBaseContext.Customer_Table.Any())
            {
                responseMessage.Status = false;
                responseMessage.Text = CommonProperties.noCustomerDataMsg;
                return responseMessage;
            }
            /// check whether there is site data or not
            if (!dataBaseContext.Site_Table.Any())
            {
                responseMessage.Status = false;
                responseMessage.Text = CommonProperties.noSiteDataMsg;
                return responseMessage;
            }
            /// check whether there is product or not
            if (!dataBaseContext.Product_Table.Any())
            {
                responseMessage.Status = false;
                responseMessage.Text = CommonProperties.noProductDataMsg;
                return responseMessage;
            }
            responseMessage.Status = true;
            return responseMessage;
        }

        /// <summary>
        /// This method will validate order information and calculate order related amount.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private Order ConvertForSaving(Order order)
        {
            var orders = new Order();
            decimal advanceBalance = decimal.Zero;
            decimal orderAmount = CalculateOrderAmount(order);

            /// getting customer balance
            advanceBalance = (from deposit in dataBaseContext.Deposit_Table
                              where deposit.CustomerID.Equals(order.CustomerID)
                              select deposit.Balance).FirstOrDefault();
           
                advanceBalance -= orderAmount;
                orders.CustomerID = order.CustomerID;
                orders.SiteID = order.SiteID;
                orders.ProductID = order.ProductID;
                orders.Quantity = order.Quantity;
                orders.OrderAmount = orderAmount;
                orders.Status = true;
                if (advanceBalance == 0)
                {
                    orders.Message = "Your available advance balance is Nu." + advanceBalance;
                }
                else
                {
                    orders.Message = "You still have an advance balance of Nu." + advanceBalance;
                }
            
            return orders;
        }
        #endregion
    }
}
