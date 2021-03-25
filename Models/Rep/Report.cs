using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NRDCL.Models.Report
{
    public class Report
    {
        [Display(Name = "Customer ID")]
        public string CustomerID { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Price Amount")]
        public decimal PriceAmount { get; set; }

        [Display(Name = "Transport Amount")]
        public decimal TransportAmount { get; set; }

        [Display(Name = "Advance Balance")]
        public decimal AdvanceBalance { get; set; }
        public int ProductID { get; set; }
        public int OrderQualtity { get; set; }
    }
}