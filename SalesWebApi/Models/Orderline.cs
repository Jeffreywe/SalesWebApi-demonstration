using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SalesWebApi.Models {
    public class Orderline {
        public int Id { get; set; }
        [Required, StringLength(30)]
        public string Product { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(7,2)")]
        public decimal Price { get; set; }

        public int OrderId { get; set; }
        [JsonIgnore] // JsonIgnore ignores reading the fk when you read the orderlines table, however, if you read orders it will read orderlines fk
        public virtual Order Order { get; set; } // lets order id be recognized as a fk

        public Orderline() { }
    }
}
