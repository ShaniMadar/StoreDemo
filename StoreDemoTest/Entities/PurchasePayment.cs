using System;
using System.Collections.Generic;

namespace StoreDemoTest.Entities
{
    public partial class PurchasePayment
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public int PaymentMethod { get; set; }
        public decimal Sum { get; set; }

        public PaymentMethod PaymentMethodNavigation { get; set; }
        public Purchase Purchase { get; set; }
    }
}
