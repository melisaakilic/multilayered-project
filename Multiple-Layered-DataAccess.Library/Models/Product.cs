﻿namespace Multiple_Layered_DataAccess.Library.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
