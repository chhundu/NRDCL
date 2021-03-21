using NRDCL.Data;
using NRDCL.Models.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NRDCL.Models
{
    public class SiteService : ISiteService
    {

        private readonly NRDCL_DB_Context dataBaseContext;
        private readonly ICustomerService customerService;
        public CommonProperties CommonProperties { get; set; }

        public SiteService(NRDCL_DB_Context context, ICustomerService service)
        {
            dataBaseContext = context;
            customerService = service;
        }
       
        public async Task<List<Site>> GetSiteList()
        {
            List<Site> siteList = (from site in dataBaseContext.Site_Table
                            join c in dataBaseContext.Customer_Table on site.CitizenshipID equals c.CitizenshipID
                           select new Site { 
                               SiteId=site.SiteId,
                               CitizenshipID=site.CitizenshipID,
                               CustomerName=c.CustomerName,
                               SiteName=site.SiteName,
                               DistanceFrom=site.DistanceFrom
                           }).ToList();
            return await Task.Run(() =>siteList);
        }
        public async Task<Site> GetSiteDetails(int SiteId)
        {
            var site = (from s in dataBaseContext.Site_Table
                        join c in dataBaseContext.Customer_Table on s.CitizenshipID equals c.CitizenshipID
                        where s.SiteId == SiteId
                        select s).SingleOrDefault();
            return await Task.FromResult(site) ;
        }

        public async Task<ResponseMessage> SaveSite(Site site)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            if (!customerService.IsCustomerExist(site.CitizenshipID).Result)
            {
                responseMessage.Status = false;
                responseMessage.Text = CommonProperties.citizenshipIDNotRegisteredMsg;
                responseMessage.MessageKey = "CitizenshipID";
                 return await Task.FromResult(responseMessage);

            }
            if (site.DistanceFrom <= 0)
            {
                responseMessage.Status = false;
                responseMessage.Text = CommonProperties.invalidSiteDistance;
                responseMessage.MessageKey = "DistanceFrom";
                return await Task.FromResult(responseMessage);
            }

            dataBaseContext.Add(site);
            dataBaseContext.SaveChanges();
            responseMessage.Status = true;
            responseMessage.Text = CommonProperties.saveSuccessMsg;
            return await Task.FromResult(responseMessage); ;
        }

        public async Task<ResponseMessage> UpdateSite(Site site)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            if (!customerService.IsCustomerExist(site.CitizenshipID).Result)
            {
                responseMessage.Status = false;
                responseMessage.Text = CommonProperties.citizenshipIDNotRegisteredMsg;
                responseMessage.MessageKey = "CitizenshipID";
                return responseMessage;
            }
            try
            {
                dataBaseContext.Update(site);
                await dataBaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!customerService.IsCustomerExist(site.CitizenshipID).Result)
                {
                    responseMessage.Status = false;
                    responseMessage.Text = CommonProperties.citizenshipIDNotRegisteredMsg;
                    return responseMessage;
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
        /// To check site exist ot not
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<bool> SiteExists(int siteId)
        {
            return await Task.FromResult(dataBaseContext.Site_Table.Any(e => e.SiteId == siteId));
        }
    }
}
