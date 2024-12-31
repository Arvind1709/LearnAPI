using LearnAPI.Model;
using LearnAPI.Repositories.IRepository;
using LearnAPI.Utilities;
using Microsoft.Data.SqlClient;
using Mono.TextTemplating;
using System.Data;

namespace LearnAPI.Repositories.Repository
{
    public class StateRepository :IStateRepository
    {
        public ResultInfo Insert(StateModel model)
        {
            return new DbTools(Info.DbConnection)
            {
                Command = "sp_state",
                SQLAction = "insert",

                Parameters = new List<DbParam>
            {
                new DbParam { Param = "state_nm", Value  = model.Name },
            }
            }.Execute();
        }

        public ResultInfo Update(StateModel model)
        {
            return new DbTools(Info.DbConnection)
            {
                Command = "sp_state",
                SQLAction = "update",

                Parameters = new List<DbParam>
            {
                new DbParam { Param = "state_id", Value  = model.Id },
                new DbParam { Param = "state_nm", Value  = model.Name },

            }
            }.Execute();
        }

        public ResultInfo Delete(int officerDesignationId)
        {
            return new DbTools(Info.DbConnection)
            {
                Command = "sp_state",
                SQLAction = "delete",

                Parameters = new List<DbParam>
            {
                new DbParam { Param = "state_id", Value  = officerDesignationId },
            }
            }.Execute();
        }

        public StateModel? Get(int officerDesignationId)
        {
            return List(officerDesignationId: officerDesignationId)?.FirstOrDefault();
        }

        
        public List<StateModel> List(int? officerDesignationId = null)
        {
            //var rInfo = new DbTools(Info.DbConnection)
            //{
            //    Command = "sp_state",
            //    SQLAction = "select",
            //    Parameters = new List<DbParam>
            //{
            //    new DbParam { Param = "state_id", Value = officerDesignationId }
            //}
            //}.ExecuteTable();

            var rInfo = new DbTools(Info.DbConnection)
            {

            }.ExecuteTable();

            // Return an empty list if there's an error or no data
            if (rInfo.HasError || rInfo.Table == null)
                return new List<StateModel>();

            var list = new List<StateModel>();

            foreach (DataRow row in rInfo.Table.Rows)
            {
                list.Add(new StateModel
                {
                    Id = row["Id"].ToInt(),
                    Name = row["Name"].ToString()
                });
            }

            return list;
        }
        /// <summary>
        /// Get states with dataAdapter using for loop
        /// </summary>
        /// <param name="officerDesignationId"></param>
        /// <returns></returns>
        public List<StateModel> GetStatesWithDataAdapter(int? officerDesignationId = null)
        {
            try
            {
                var list = new List<StateModel>();
                using(SqlConnection cn =  new SqlConnection(Info.DbConnection))
                {
                    var query = "select Id,[Name] from States";
                    cn.Open();
                    // DataAdapter to fill a DataTable
                    using (var dataAdapter = new SqlDataAdapter(query, cn))
                    {
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable); // Fill the DataTable with query results

                        // Iterate through DataTable rows to populate the list
                        foreach (DataRow row in dataTable.Rows)
                        {
                            list.Add(new StateModel
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Name = row["Name"].ToString()
                            });
                        }
                    }

                }
                return list.ToList();
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error: {ex.Message}");
                // Log or handle the exception as needed
                throw new Exception($"{ex.Message}");
            }
            
        }
        /// <summary>
        /// Get states with dataReader using while loop
        /// </summary>
        /// <returns>List of states</returns>

        public List<StateModel> GetStatesWithDataReader()
        {
            var states = new List<StateModel>();

            try
            {
                using (var connection = new SqlConnection(Info.DbConnection))
                {
                    connection.Open();

                    // SQL query to fetch states
                    string query = "SELECT Id, Name FROM State";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Populate StateModel object
                                states.Add(new StateModel
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Name"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error: {ex.Message}");
                // Log or handle the exception as needed
                throw new Exception($"{ex.Message}");
            }

            return states;
        }

        /// <summary>
        /// GetStates with DataAdapter And StoredProcedure
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public List<StateModel> GetStatesDataAdapterAndStoredProcedure(int? stateId = null)
        {
            var states = new List<StateModel>();

            try
            {
                using (var connection = new SqlConnection(Info.DbConnection))
                {
                    connection.Open();

                    // Create a SqlDataAdapter with the stored procedure
                    using (var dataAdapter = new SqlDataAdapter("sp_state", connection))
                    {
                        dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                        // Add the parameter to the stored procedure
                        dataAdapter.SelectCommand.Parameters.AddWithValue("@state_id", stateId);
                        dataAdapter.SelectCommand.Parameters.AddWithValue("@name", string.Empty);

                        // Fill the DataTable with the results
                        var dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        // Iterate through DataTable rows to populate the list
                        foreach (DataRow row in dataTable.Rows)
                        {
                            states.Add(new StateModel
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Name = row["Name"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error: {ex.Message}");
                // Log or handle the exception as needed
                throw new Exception($"{ex.Message}");
            }

            return states;
        }

    }
}
