using System;
using System.Collections.Generic;

namespace StoreDemoTest.Entities
{
    public partial class Employees
    {
        public Employees()
        {
            Inventory = new HashSet<Inventory>();
            Purchase = new HashSet<Purchase>();
            Returns = new HashSet<Returns>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ICollection<Inventory> Inventory { get; set; }
        public ICollection<Purchase> Purchase { get; set; }
        public ICollection<Returns> Returns { get; set; }
    }
}
