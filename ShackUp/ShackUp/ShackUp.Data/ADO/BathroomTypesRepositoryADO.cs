using ShackUp.Data.Interfaces;
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
    public class BathroomTypesRepositoryADO : IBathroomTypesRepository
    {
        public List<BathroomType> GetAll()
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                List<BathroomType> bathroomTypes = new List<BathroomType>();
                SqlCommand cmd = new SqlCommand("BathroomTypesSelectAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        BathroomType currentRow = new BathroomType();
                        currentRow.BathroomTypeId = (int)dr["BathroomTypeId"];
                        currentRow.BathroomTypeName = dr["BathroomTypeName"].ToString();

                        bathroomTypes.Add(currentRow);
                    }
                }

                return bathroomTypes;
            }
        }
    }
}
