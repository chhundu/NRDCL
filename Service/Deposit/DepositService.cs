using Microsoft.EntityFrameworkCore;
using NRDCL.Data;
using NRDCL.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Models
{
    public class DepositService : IDepositService
    {
        private readonly NRDCL_DB_Context dataBaseContext;
        private readonly ICustomerService customerService;
        public CommonProperties CommonProperties { get; set; }

        public DepositService(NRDCL_DB_Context context, ICustomerService service)
        {
            dataBaseContext = context;
            customerService = service;
        }

        /// <summary>
        /// To get deposit details
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public Deposit GetDepositDetails(String customerID)
        {
            var deposit = (from dep in dataBaseContext.Deposit_Table where dep.CustomerID.Equals(customerID) select dep).SingleOrDefault();

            return deposit;
        }

        /// <summary>
        /// To get deposit list
        /// </summary>
        /// <returns></returns>
        public List<Deposit> GetDepositList()
        {
            List<Deposit> depositList = dataBaseContext.Deposit_Table.ToList();
            return depositList;
        }

        /// <summary>
        /// Save deposit information
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ResponseMessage SaveDeposit(Deposit deposit)
        {

            ResponseMessage responseMessage = new ResponseMessage();
            Decimal totalBalance;

            if (!customerService.IsCustomerExist(deposit.CustomerID))
            {
                responseMessage.Status = false;
                responseMessage.Text = CommonProperties.citizenshipIDNotRegisteredMsg;
                responseMessage.MessageKey = "CustomerID";
                return responseMessage;

            }

            if (deposit.LastAmount <= 0) {
                responseMessage.Status = false;
                responseMessage.Text = CommonProperties.invalidDepositAmountMsg;
                responseMessage.MessageKey = "LastAmount";
                return responseMessage;
            }
            /// checking if there is already data or not.
            if (dataBaseContext.Deposit_Table.Find(deposit.CustomerID)==null)
            {
                decimal balance;
                if (deposit.Balance == 0)
                {
                    balance = deposit.LastAmount;
                }
                else {
                    balance = deposit.Balance;
                }
                deposit.Balance = balance;
                dataBaseContext.Add(deposit);
                totalBalance = deposit.Balance;
            }
            else
            {
                /// calculate total balance after depositing amount.
                totalBalance = CalculateTotalBalance(deposit);
                deposit.Balance = totalBalance;
                deposit.LastAmount = deposit.LastAmount;

                /// deleting/removing previous data.
                DeleteDeposit(deposit.CustomerID);

                /// saving/updating advance balance
                dataBaseContext.Add(deposit);
            }

            dataBaseContext.SaveChanges();
            responseMessage.Status = true;
            responseMessage.Text = CommonProperties.saveSuccessMsg + " Your Current balance is Nu. " + totalBalance;

            return responseMessage;
        }

        /// <summary>
        /// Delete deposit data
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public void DeleteDeposit(string customerID)
        {
            var deposit = dataBaseContext.Deposit_Table.Find(customerID);
            dataBaseContext.Deposit_Table.Remove(deposit);
            dataBaseContext.SaveChanges();
        }


        public bool DepositExists(string customerID)
        {
            return dataBaseContext.Deposit_Table.Any(e => e.CustomerID.Equals(customerID));
        }

        /// <summary>
        /// This method calculates the total balance of customer.
        /// </summary>
        /// <param name="deposit"></param>
        /// <param name="depositDictionary"></param>
        /// <returns>totalBalance</returns>
        private decimal CalculateTotalBalance(Deposit deposit)
        {
            decimal totalBalance = deposit.LastAmount;
            if (dataBaseContext.Deposit_Table.Any(e => e.CustomerID.Equals(deposit.CustomerID)))
            {
                totalBalance += (from dep in dataBaseContext.Deposit_Table where dep.CustomerID.Equals(deposit.CustomerID) select dep.Balance).FirstOrDefault();
            }
            return totalBalance;
        }
    }
}
