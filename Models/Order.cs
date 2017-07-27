using System;

namespace IEvangelist.Web.Api.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }
    }
}