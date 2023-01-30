using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataService.Models
{
    public class Product
    {
        public Guid ID { get; set; }
        public Guid? Account { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? EAN { get; set; }
        public DateTime LastModified { get; set; }
    }
}