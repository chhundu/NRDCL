using NRDCL.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Models
{
    public interface IDepositService
    {
        List<Deposit> GetDepositList();
        Deposit GetDepositDetails(string CitizenshipID);
        ResponseMessage SaveDeposit(Deposit deposit);
        bool DepositExists(string customerID);
    }
}
