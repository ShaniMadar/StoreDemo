using System;
using System.Collections.Generic;

namespace StoreDemoTest.Entities
{
    public partial class Inventory
    {
        public int Id { get; set; }
        public int Item { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
        public int? Employee { get; set; }

        public Employees EmployeeNavigation { get; set; }
        public Items ItemNavigation { get; set; }
    }
}
