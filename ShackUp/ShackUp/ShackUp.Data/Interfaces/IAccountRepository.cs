using ShackUp.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShackUp.Data.Interfaces
{
    public interface IAccountRepository
    {
        IEnumerable<FavoriteItem> GetFavorites(string userId);
        IEnumerable<ContactRequestItem> GetContacts(string userId);
        IEnumerable<ListingItem> GetListings(string userId);
        void AddFavorite(string userId, int listingId);
        void RemoveFavorite(string userId, int listingId);
        void AddContact(string userId, int listingId);
        void RemoveContact(string userId, int listingId);
        bool IsFavorite(string userId, int listingId);
        bool IsContact(string userId, int listingId);
    }
}
