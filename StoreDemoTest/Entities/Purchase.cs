using System;
using System.Collections.Generic;

namespace StoreDemoTest.Entities
{
    public partial class Purchase
    {
        public Purchase()
        {
            PurchaseDetails = new HashSet<PurchaseDetails>();
            PurchasePayment = new HashSet<PurchasePayment>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Employee { get; set; }
        public int Status { get; set; }
        public decimal TotalSum { get; set; }
        public decimal? PaidAmount { get; set; }

        public Employees EmployeeNavigation { get; set; }
        public PurchaseStatusType StatusNavigation { get; set; }
        public ICollection<PurchaseDetails> PurchaseDetails { get; set; }
        public ICollection<PurchasePayment> PurchasePayment { get; set; }
    }
}
