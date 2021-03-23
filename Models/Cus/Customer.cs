using NRDCL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NRDCL.Models
{
    public class Customer
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.Sites = new HashSet<Site>();
            this.Orders = new HashSet<Order>();
        }
        [Key]
        [Required(ErrorMessage = "Citizenship ID is mendatory.")]
        [Display(Name = "Citizenship ID")]
        public string CitizenshipID { get; set; }

        [Required(ErrorMessage = "Customer name is mendatory.")]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Telephone number is mendatory.")]
        [Display(Name = "Telephone Number")]
        public string TelephoneNumber { get; set; }
        
        [Display(Name = "Email ID")]
        public string EmailId { get; set; }

        [NotMapped]
        public string CMDstatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Site> Sites { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        public virtual Deposit Deposit { get; set; }

    }
}