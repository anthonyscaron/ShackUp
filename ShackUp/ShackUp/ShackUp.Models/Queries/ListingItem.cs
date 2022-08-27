using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShackUp.Models.Queries
{
    public class ListingItem
    {
        public int ListingId { get; set; }
        public string UserId { get; set; }
        public string Nickname { get; set; }
        public string City { get; set; }
        public string StateId { get; set; }
        public decimal Rate { get; set; }
        public decimal SquareFootage { get; set; }
        public bool HasElectric { get; set; }
        public bool HasHeat { get; set; }
        public int BathroomTypeId { get; set; }
        public string BathroomTypeName { get; set; }
        public string ListingDescription { get; set; }
        public string ImageFileName { get; set; }
    }
}
