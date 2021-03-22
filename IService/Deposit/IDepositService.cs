using NRDCL.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Models
{
    public interface IDepositService
    {
        Task<List<Deposit>> GetDepositList();
        Task<Deposit> GetDepositDetails(string CitizenshipID);
        Task<ResponseMessage> SaveDeposit(Deposit deposit);
        bool DepositExists(string customerID);
        void DeleteDeposit(string customerID);
    }
}
