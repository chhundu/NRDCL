using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRDCL.Models
{
    public class Site
    {
        public Site()
        {
            this.Orders = new HashSet<Order>();
        }
        public int SiteId { get; set; }

        [ForeignKey("Customer")]
        public string CitizenshipID { get; set; }
        public string SiteName { get; set; }
        public double DistanceFrom { get; set; }

        [NotMapped]
        public string CustomerName { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}