using NRDCL.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NRDCL.Models
{
    public interface ISiteService
    {
        Task<List<Site>> GetSiteList();
        Task<Site> GetSiteDetails(int siteId);
        Task<ResponseMessage> SaveSite(Site site);
        Task<ResponseMessage> UpdateSite(Site site);
        Task<bool> SiteExists(int siteId);
    }
}
