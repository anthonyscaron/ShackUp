using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShackUp.Models.Queries
{
    public class ListingSearchParameters
    {
        public decimal? MinRate { get; set; }
        public decimal? MaxRate { get; set; }
        public string City { get; set; }
        public string StateId { get; set; }
    }
}
