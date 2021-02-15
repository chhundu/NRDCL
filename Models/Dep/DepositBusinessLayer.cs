using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NRDCL.Models
{
    public class DepositBusinessLayer
    {
       // public NRDCL_DB_Entities dbContext = new NRDCL_DB_Entities();

        public List<Deposit> GetDepositList()
        {
            List<Deposit> depositList = new List<Deposit>();
            Deposit deposit = new Deposit();
            deposit.CustomerID = "1";
            deposit.LastAmount = Decimal.Zero;
            deposit.Balance = decimal.Zero;
            depositList.Add(deposit);

           // List<Deposit> depositList = dbContext.Deposits.ToList();
            return depositList;
        }

        public Deposit GetDepositDetail(string CustomerID)
        {
            Deposit deposit = new Deposit();
            deposit.CustomerID = "1";
            deposit.LastAmount = Decimal.Zero;
            deposit.Balance = decimal.Zero;

            //var deposit = (from d in dbContext.Deposits
            //               join c in dbContext.Customers on d.CustomerID equals c.CitizenshipID
            //               where d.CustomerID == CustomerID
            //               select d).FirstOrDefault();
            return deposit;
        }
    }
}