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
    public class StatesRepositoryADO : IStatesRepository
    {
        public List<State> GetAll()
        {
            using(var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                List<State> states = new List<State>();
                SqlCommand cmd = new SqlCommand("StatesSelectAll", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        State currentRow = new State();
                        currentRow.StateId = dr["StateId"].ToString();
                        currentRow.StateName = dr["StateName"].ToString();

                        states.Add(currentRow);
                    }
                }

                return states;
            }
        }
    }
}
