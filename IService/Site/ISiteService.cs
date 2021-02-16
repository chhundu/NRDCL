using NRDCL.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Models
{
    public interface ISiteService
    {
        List<Site> GetSiteList();
        Site GetSiteDetails(int siteId);
        ResponseMessage SaveSite(Site site);
        ResponseMessage UpdateSite(Site site);
        bool SiteExists(int siteId);
    }
}
