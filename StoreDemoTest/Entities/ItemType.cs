using System;
using System.Collections.Generic;

namespace StoreDemoTest.Entities
{
    public partial class ItemType
    {
        public ItemType()
        {
            Items = new HashSet<Items>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ReturnPeriod { get; set; }

        public ICollection<Items> Items { get; set; }
    }
}
