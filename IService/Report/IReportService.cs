using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRDCL.Models.Report
{
    public interface IReportService
    {
        List<Report> GetReportData(int customerReportNo);
    }
}
