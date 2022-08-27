using ShackUp.Data.Interfaces;
using ShackUp.Models.Queries;
using ShackUp.Models.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShackUp.Data.ADO
{
    public class ListingsRepositoryADO : IListingsRepository
    {
        public void Delete(int listingId)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("ListingsDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ListingId", listingId);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public Listing GetById(int listingId)
        {
            Listing listing = null;

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("ListingsSelect", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ListingId", listingId);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        listing = new Listing();
                        listing.ListingId = (int)dr["ListingId"];
                        listing.UserId = dr["UserId"].ToString();
                        listing.StateId = dr["StateId"].ToString();
                        listing.BathroomTypeId = (int)dr["BathroomTypeId"];
                        listing.Nickname = dr["Nickname"].ToString();
                        listing.City = dr["City"].ToString();
                        listing.Rate = (decimal)dr["Rate"];
                        listing.SquareFootage = (decimal)dr["SquareFootage"];
                        listing.HasElectric = (bool)dr["HasElectric"];
                        listing.HasHeat = (bool)dr["HasHeat"];

                        if (dr["ListingDescription"] != DBNull.Value)
                        {
                            listing.ListingDescription = dr["ListingDescription"].ToString();
                        }

                        if (dr["ImageFileName"] != DBNull.Value)
                        {
                            listing.ImageFileName = dr["ImageFileName"].ToString();
                        }
                    }
                }
            }

            return listing;
        }

        public ListingItem GetDetails(int listingId)
        {
            ListingItem listing = null;

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("ListingsSelectDetails", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ListingId", listingId);

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        listing = new ListingItem();
                        listing.ListingId = (int)dr["ListingId"];
                        listing.UserId = dr["UserId"].ToString();
                        listing.StateId = dr["StateId"].ToString();
                        listing.BathroomTypeId = (int)dr["BathroomTypeId"];
                        listing.Nickname = dr["Nickname"].ToString();
                        listing.City = dr["City"].ToString();
                        listing.Rate = (decimal)dr["Rate"];
                        listing.SquareFootage = (decimal)dr["SquareFootage"];
                        listing.HasElectric = (bool)dr["HasElectric"];
                        listing.HasHeat = (bool)dr["HasHeat"];
                        listing.BathroomTypeName = dr["BathroomTypeName"].ToString();

                        if (dr["ListingDescription"] != DBNull.Value)
                        {
                            listing.ListingDescription = dr["ListingDescription"].ToString();
                        }

                        if (dr["ImageFileName"] != DBNull.Value)
                        {
                            listing.ImageFileName = dr["ImageFileName"].ToString();
                        }
                    }
                }
            }

            return listing;
        }

        public IEnumerable<ListingShortItem> GetRecent()
        {
            List<ListingShortItem> listings = new List<ListingShortItem>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("ListingsSelectRecent", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ListingShortItem row = new ListingShortItem();

                        row.ListingId = (int)dr["ListingId"];
                        row.UserId = dr["UserId"].ToString();
                        row.StateId = dr["StateId"].ToString();
                        row.City = dr["City"].ToString();
                        row.Rate = (decimal)dr["Rate"];

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

        public void Insert(Listing listing)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("ListingsInsert", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter("@ListingId", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(param);

                cmd.Parameters.AddWithValue("@UserId", listing.UserId);
                cmd.Parameters.AddWithValue("@StateId", listing.StateId);
                cmd.Parameters.AddWithValue("@BathroomTypeId", listing.BathroomTypeId);
                cmd.Parameters.AddWithValue("@Nickname", listing.Nickname);
                cmd.Parameters.AddWithValue("@City", listing.City);
                cmd.Parameters.AddWithValue("@Rate", listing.Rate);
                cmd.Parameters.AddWithValue("@SquareFootage", listing.SquareFootage);
                cmd.Parameters.AddWithValue("@HasElectric", listing.HasElectric);
                cmd.Parameters.AddWithValue("@HasHeat", listing.HasHeat);

                if (string.IsNullOrEmpty(listing.ImageFileName))
                {
                    cmd.Parameters.AddWithValue("@ImageFileName", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ImageFileName", listing.ImageFileName);
                }

                cmd.Parameters.AddWithValue("@ListingDescription", listing.ListingDescription);

                cn.Open();

                cmd.ExecuteNonQuery();

                listing.ListingId = (int)param.Value;
            }
        }

        public IEnumerable<ListingShortItem> Search(ListingSearchParameters parameters)
        {
            List<ListingShortItem> listings = new List<ListingShortItem>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                string query = "SELECT TOP 12 ListingId, UserId, City, StateId, Rate, ImageFileName FROM Listings WHERE 1 = 1 ";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                
                if (parameters.MinRate.HasValue)
                {
                    query += "AND Rate >= @MinRate ";
                    cmd.Parameters.AddWithValue("@MinRate", parameters.MinRate.Value);
                }

                if (parameters.MaxRate.HasValue)
                {
                    query += "AND Rate <= @MaxRate ";
                    cmd.Parameters.AddWithValue("@MaxRate", parameters.MaxRate.Value);
                }

                if (!string.IsNullOrEmpty(parameters.City))
                {
                    query += "AND City LIKE @City ";
                    cmd.Parameters.AddWithValue("@City", parameters.City + '%');
                }

                if (!string.IsNullOrEmpty(parameters.StateId))
                {
                    query += "AND StateId = @StateId ";
                    cmd.Parameters.AddWithValue("@StateId", parameters.StateId);
                }

                query += "ORDER BY CreatedDate DESC";
                cmd.CommandText = query;

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ListingShortItem row = new ListingShortItem();

                        row.ListingId = (int)dr["ListingId"];
                        row.UserId = dr["UserId"].ToString();
                        row.StateId = dr["StateId"].ToString();
                        row.City = dr["City"].ToString();
                        row.Rate = (decimal)dr["Rate"];

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

        public void Update(Listing listing)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("ListingsUpdate", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ListingId", listing.ListingId);
                cmd.Parameters.AddWithValue("@UserId", listing.UserId);
                cmd.Parameters.AddWithValue("@StateId", listing.StateId);
                cmd.Parameters.AddWithValue("@BathroomTypeId", listing.BathroomTypeId);
                cmd.Parameters.AddWithValue("@Nickname", listing.Nickname);
                cmd.Parameters.AddWithValue("@City", listing.City);
                cmd.Parameters.AddWithValue("@Rate", listing.Rate);
                cmd.Parameters.AddWithValue("@SquareFootage", listing.SquareFootage);
                cmd.Parameters.AddWithValue("@HasElectric", listing.HasElectric);
                cmd.Parameters.AddWithValue("@HasHeat", listing.HasHeat);
                cmd.Parameters.AddWithValue("@ListingDescription", listing.ListingDescription);
                cmd.Parameters.AddWithValue("@ImageFileName", listing.ImageFileName);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
        }
    }
}
