using ShackUp.Data.Interfaces;
using ShackUp.Models.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShackUp.Data.ADO
{
    public class AccountRepositoryADO : IAccountRepository
    {
        public void AddContact(string userId, int listingId)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("ContactsInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ListingId", listingId);
                cmd.Parameters.AddWithValue("@UserId", userId);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void AddFavorite(string userId, int listingId)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("FavoritesInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ListingId", listingId);
                cmd.Parameters.AddWithValue("@UserId", userId);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<ContactRequestItem> GetContacts(string userId)
        {
            List<ContactRequestItem> contacts = new List<ContactRequestItem>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("ListingsSelectContacts", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ContactRequestItem row = new ContactRequestItem();

                        row.ListingId = (int)dr["ListingId"];
                        row.UserId = dr["UserId"].ToString();
                        row.StateId = dr["StateId"].ToString();
                        row.City = dr["City"].ToString();
                        row.Rate = (decimal)dr["Rate"];
                        row.Email = dr["Email"].ToString();
                        row.Nickname = dr["Nickname"].ToString();

                        contacts.Add(row);
                    }
                }
            }

            return contacts;
        }

        public IEnumerable<FavoriteItem> GetFavorites(string userId)
        {
            List<FavoriteItem> listings = new List<FavoriteItem>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("ListingsSelectFavorites", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        FavoriteItem row = new FavoriteItem();

                        row.ListingId = (int)dr["ListingId"];
                        row.UserId = dr["UserId"].ToString();
                        row.StateId = dr["StateId"].ToString();
                        row.City = dr["City"].ToString();
                        row.Rate = (decimal)dr["Rate"];
                        row.BathroomTypeId = (int)dr["BathroomTypeId"];
                        row.BathroomTypeName = dr["BathroomTypeName"].ToString();
                        row.SquareFootage = (decimal)dr["SquareFootage"];
                        row.HasElectric = (bool)dr["HasElectric"];
                        row.HasHeat = (bool)dr["HasHeat"];

                        listings.Add(row);
                    }
                }
            }

            return listings;
        }

        public IEnumerable<ListingItem> GetListings(string userId)
        {
            List<ListingItem> listings = new List<ListingItem>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("ListingsSelectByUser", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ListingItem row = new ListingItem();

                        row.ListingId = (int)dr["ListingId"];
                        row.UserId = dr["UserId"].ToString();
                        row.StateId = dr["StateId"].ToString();
                        row.City = dr["City"].ToString();
                        row.Rate = (decimal)dr["Rate"];
                        row.Nickname = dr["Nickname"].ToString();
                        row.SquareFootage = (decimal)dr["Squarefootage"];
                        row.HasElectric = (bool)dr["HasElectric"];
                        row.HasHeat = (bool)dr["HasHeat"];
                        row.BathroomTypeId = (int)dr["BathroomTypeId"];
                        row.BathroomTypeName = dr["BathroomTypeName"].ToString();

                        if (dr["ImageFileName"] != DBNull.Value)
                        {
                            row.ImageFileName = dr["ImageFileName"].ToString();
                        }

                        listings.Add(row);
                    }
                }
            }

            return listings;
        }

        public bool IsContact(string userId, int listingId)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("ContactsSelect", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ListingId", listingId);
                cmd.Parameters.AddWithValue("@UserId", userId);

                cn.Open();

                using(var dr = cmd.ExecuteReader())
                {
                    return dr.Read();
                }
            }
        }

        public bool IsFavorite(string userId, int listingId)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("FavoritesSelect", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ListingId", listingId);
                cmd.Parameters.AddWithValue("@UserId", userId);

                cn.Open();

                using (var dr = cmd.ExecuteReader())
                {
                    return dr.Read();
                }
            }
        }

        public void RemoveContact(string userId, int listingId)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("ContactsDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ListingId", listingId);
                cmd.Parameters.AddWithValue("@UserId", userId);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveFavorite(string userId, int listingId)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("FavoritesDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ListingId", listingId);
                cmd.Parameters.AddWithValue("@UserId", userId);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
        }
    }
}
