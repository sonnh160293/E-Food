﻿using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOnline.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Weight { get; set; }
        public int? Volume { get; set; }
        public double? UnitPrice { get; set; }
        public int? Discount { get; set; }
        public string? ImageURL { get; set; }


        public bool? IsActice { get; set; }
        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; } = null!;

    }
}
