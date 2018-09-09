using System;
using System.Collections.Generic;

namespace StoreDemoTest.Entities
{
    public partial class PurchaseDetails
    {
        public PurchaseDetails()
        {
            Returns = new HashSet<Returns>();
        }

        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public int Item { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }

        public Items ItemNavigation { get; set; }
        public Purchase Purchase { get; set; }
        public PurchaseStatusType StatusNavigation { get; set; }
        public ICollection<Returns> Returns { get; set; }
    }
}
