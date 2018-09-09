using System;
using System.Collections.Generic;

namespace StoreDemoTest.Entities
{
    public partial class PurchaseStatusType
    {
        public PurchaseStatusType()
        {
            Purchase = new HashSet<Purchase>();
            PurchaseDetails = new HashSet<PurchaseDetails>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Purchase> Purchase { get; set; }
        public ICollection<PurchaseDetails> PurchaseDetails { get; set; }
    }
}
