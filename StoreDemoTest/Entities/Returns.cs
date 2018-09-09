using System;
using System.Collections.Generic;

namespace StoreDemoTest.Entities
{
    public partial class Returns
    {
        public Returns()
        {

        }

        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public int CreditType { get; set; }
        public int Employee { get; set; }
        public DateTime Date { get; set; }
        public int PurchaseDetailId { get; set; }
        public int Quantity { get; set; }

        public PaymentMethod CreditTypeNavigation { get; set; }
        public Employees EmployeeNavigation { get; set; }
        public PurchaseDetails PurchaseDetail { get; set; }
    }
}
