using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "Citizenship ID is mendatory.")]
        [Display(Name = "Citizenship ID")]
        public string CitizenshipID { get; set; }

        [Required(ErrorMessage = "Site name is mendatory.")]
        [Display(Name = "Site Name")]
        public string SiteName { get; set; }

        [Required(ErrorMessage = "Distance from is mendatory.")]
        [Display(Name = "Distance From")]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public double DistanceFrom { get; set; }

        [NotMapped]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}