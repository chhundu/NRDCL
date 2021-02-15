using NRDCL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public string CitizenshipID { get; set; }
        public string CustomerName { get; set; }
        public string TelephoneNumber { get; set; }
        public string EmailId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Site> Sites { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        public virtual Deposit Deposit { get; set; }

    }
}