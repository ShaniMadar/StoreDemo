using System;
using System.Collections.Generic;

namespace StoreDemoTest.Entities
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            PurchasePayment = new HashSet<PurchasePayment>();
            Returns = new HashSet<Returns>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<PurchasePayment> PurchasePayment { get; set; }
        public ICollection<Returns> Returns { get; set; }
    }
}
