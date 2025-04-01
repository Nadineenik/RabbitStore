using System;


namespace laba2.Models
{
    public class Rabbit
    {
        public int Id { get; set; }  
        public string Name { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty; 
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}

