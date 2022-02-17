﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebApi.Models {
    public class Order {

        public int Id { get; set; }
        [Required, StringLength(80)]
        public string Description { get; set; }
        public bool Shipped { get; set; }
        [Column(TypeName = "decimal(7,2)")]
        public decimal Total { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; } // this statement EF recognizes CustomerId as a foreign key

        public virtual IEnumerable<Orderline> Orderlines { get; set; } // fills all the line items associated with an order when we read an order
            // virtual means its only in the class not in the database table
        public Order() { }
    }
}
