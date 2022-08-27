using ShackUp.Models.Queries;
using ShackUp.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShackUp.Data.Interfaces
{
    public interface IListingsRepository
    {
        Listing GetById(int listingId);
        void Insert(Listing listing);
        void Update(Listing listing);
        void Delete(int listingId);
        IEnumerable<ListingShortItem> GetRecent();
        ListingItem GetDetails(int listingId);
        IEnumerable<ListingShortItem> Search(ListingSearchParameters parameters);
    }
}
