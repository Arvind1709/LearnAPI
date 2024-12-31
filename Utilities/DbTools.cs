using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace LearnAPI.Utilities
{
    public class DbTools
    {
        public SqlConnection DbConnection { get; set; } = new SqlConnection();


        public SqlCommand DbCommand { get; set; } = new SqlCommand();


        public SqlTransaction Transaction { get; set; } = null;


        public string Command
        {
            set
            {
                DbCommand.CommandText = value;
            }
        }

        public string SQLAction
        {
            set
            {
                DbCommand.Parameters.AddWithValue("sql_action", value);
            }
        }

        public List<DbParam> Parameters
        {
            set
            {
                foreach (DbParam item in value)
                {
                    DbCommand.Parameters.AddWithValue(item.Param, item.Value);
                    if (item.DbType.HasValue)
                    {
                        DbCommand.Parameters[item.Param].SqlDbType = item.DbType.Value;
                    }
                }
            }
        }

        public DbTools(string connectionString = "", CommandType commandType = CommandType.StoredProcedure)
        {
            DbConnection.ConnectionString = connectionString;
            DbCommand.Connection = DbConnection;
            DbCommand.CommandType = commandType;
        }

        public void Open(bool transactionHandling = false)
        {
            if (DbConnection.State != ConnectionState.Open)
            {
                DbConnection.Open();
            }

            DbCommand.Connection = DbConnection;
            if (transactionHandling)
            {
                Transaction = DbConnection.BeginTransaction();
                DbCommand.Transaction = Transaction;
            }
        }

        public void BeginTransaction()
        {
            Transaction = DbConnection.BeginTransaction();
            DbCommand.Transaction = Transaction;
        }

        public void Commit()
        {
            Transaction.Commit();
        }

        public void Rollback()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
            }
        }

        public void Close(bool commitTransaction = false)
        {
            if (commitTransaction)
            {
                Commit();
            }

            if (DbConnection.State == ConnectionState.Open)
            {
                DbConnection.Close();
            }
        }

        public void AssignParam(string parameterName, object value)
        {
            DbCommand.Parameters[parameterName].Value = ((value == null || value.ToString() == "") ? null : value);
        }

        public void ClearParameters()
        {
            DbCommand.Parameters.Clear();
        }

        public ResultInfo Execute(bool openConnection = true, bool closeConnection = true)
        {
            ResultInfo resultInfo = new ResultInfo();
            try
            {
                if (openConnection)
                {
                    Open();
                }

                DbCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                resultInfo.SetError(ex.Message, ex.Number, handyError: true);
            }
            catch (Exception ex2)
            {
                resultInfo.SetError(ex2.Message, 0, handyError: true);
            }
            finally
            {
                if (closeConnection)
                {
                    Close();
                }
            }

            return resultInfo;
        }

        public ResultInfo ExecuteScaler(bool openConnection = true, bool closeConnection = true)
        {
            ResultInfo resultInfo = new ResultInfo();
            try
            {
                if (openConnection)
                {
                    Open();
                }

                object obj = DbCommand.ExecuteScalar();
                if (obj == null)
                {
                    resultInfo.Result = "";
                }
                else
                {
                    resultInfo.Result = obj.ToString();
                }
            }
            catch (SqlException ex)
            {
                resultInfo.SetError(ex.Message, ex.Number, handyError: true);
            }
            catch (Exception ex2)
            {
                resultInfo.SetError(ex2.Message, 0, handyError: true);
            }
            finally
            {
                if (closeConnection)
                {
                    Close();
                }
            }

            return resultInfo;
        }

        public SqlDataReader ExecuteReader()
        {
            return DbCommand.ExecuteReader();
        }

        public ResultInfo ExecuteTable()
        {
            ResultInfo resultInfo = new ResultInfo();
            try
            {
                resultInfo.Table = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(DbCommand);
                sqlDataAdapter.Fill(resultInfo.Table);
            }
            catch (SqlException ex)
            {
                resultInfo.SetError(ex.Message, ex.Number, handyError: true);
            }
            catch (Exception ex2)
            {
                resultInfo.SetError(ex2.Message, 0, handyError: true);
            }

            return resultInfo;
        }

        public ResultInfo ExecuteDataSet()
        {
            ResultInfo resultInfo = new ResultInfo();
            try
            {
                DataSet dataSet = new DataSet();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(DbCommand);
                sqlDataAdapter.Fill(dataSet);
                resultInfo.Object = dataSet;
            }
            catch (SqlException ex)
            {
                resultInfo.SetError(ex.Message, ex.Number, handyError: true);
            }
            catch (Exception ex2)
            {
                resultInfo.SetError(ex2.Message, 0, handyError: true);
            }

            return resultInfo;
        }
    }
}
