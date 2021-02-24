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
            dataBaseContext = context;
            depositService = deposit;
        }
        public List<Order> GetOrderList()
        {
            List<Order> orderList = (from ord in dataBaseContext.Order_Table
                                     join cus in dataBaseContext.Customer_Table on ord.CustomerID equals cus.CitizenshipID
                                     join site in dataBaseContext.Site_Table on ord.SiteID equals site.SiteId
                                     join p in dataBaseContext.Product_Table on ord.ProductID equals p.ProductId
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
                                         OrderAmount=ord.OrderAmount
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

            /// validate order information and calculate order related amount.
            Order validatedOrderInformation = ValidateOrderAndCalculate(order);
            if (!validatedOrderInformation.Status)
            {
                responseMessage.Status = validatedOrderInformation.Status;
                responseMessage.Text = validatedOrderInformation.Message;
                return responseMessage;
            }

            /// saving and updating amount in deposite table
            var deposits = new Deposit();
            deposits.CustomerID = validatedOrderInformation.CustomerID;
            deposits.LastAmount = (from deposit in dataBaseContext.Deposit_Table
                                   where deposit.CustomerID.Equals(validatedOrderInformation.CustomerID)
                                   select deposit.LastAmount).FirstOrDefault();

            deposits.Balance = (from deposit in dataBaseContext.Deposit_Table
                                where deposit.CustomerID.Equals(validatedOrderInformation.CustomerID)
                                select deposit.Balance).FirstOrDefault() - (validatedOrderInformation.OrderAmount);

            depositService.DeleteDeposit(validatedOrderInformation.CustomerID);
            depositService.SaveDeposit(deposits);

            /// saving/updating order
            dataBaseContext.Add(validatedOrderInformation);
            dataBaseContext.SaveChanges();
            responseMessage.Text = validatedOrderInformation.Message;
            return responseMessage;
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
        private Order ValidateOrderAndCalculate(Order order)
        {
            var orders = new Order();
            decimal orderAmount=decimal.Zero;
            decimal advanceBalance= decimal.Zero;
            decimal unitPrice= decimal.Zero;
            decimal transportRate= decimal.Zero;
            double distanceFrom=0.0;
            decimal additionalAmountRequired= decimal.Zero;

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

            if (advanceBalance < orderAmount)
            {
                /// amount that is needed to place an order.
                additionalAmountRequired = orderAmount - advanceBalance;
                orders.Status = false;
                orders.Message = Math.Abs(additionalAmountRequired) + " additional amount is required to place the order.";
            }
            else
            {
                advanceBalance -= orderAmount;
                orders.CustomerID = customerID;
                orders.SiteID = siteID;
                orders.ProductID = productID;
                orders.Quantity = quantity;
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
            }
            return orders;
        }

        /// <summary>
        /// Delete order data
        /// </summary>
        /// <param name="customerID"></param>
        private void DeleteDeposit(string customerID)
        {
            var order = dataBaseContext.Order_Table.Find(customerID);
            dataBaseContext.Order_Table.Remove(order);
            dataBaseContext.SaveChanges();
        }
        #endregion
    }
}
