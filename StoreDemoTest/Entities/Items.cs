using System;
using System.Collections.Generic;

namespace StoreDemoTest.Entities
{
    public partial class Items
    {
        public Items()
        {
            Inventory = new HashSet<Inventory>();
            PurchaseDetails = new HashSet<PurchaseDetails>();
        }

        public int Id { get; set; }
        public int ItemType { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public ItemType ItemTypeNavigation { get; set; }
        public ICollection<Inventory> Inventory { get; set; }
        public ICollection<PurchaseDetails> PurchaseDetails { get; set; }
    }
}
