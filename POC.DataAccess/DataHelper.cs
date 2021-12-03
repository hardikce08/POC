using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace POC.DataAccess
{
    public class DataHelper : ConnectionHelper
    {
        const int COMMAND_TIMEOUT = 30 * 20;

        #region Common Methods to Execute Stored Functions

        public DataSet ExecuteStoredProcedure(string procedureName, SqlParameter[] para, CommandType commandType = CommandType.StoredProcedure)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[DbConnectionStringKey].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = COMMAND_TIMEOUT;
                cmd.CommandType = commandType;
                cmd.Connection = connection;
                cmd.Connection.Open();

                cmd.CommandText = procedureName;

                if (para != null && para.Length > 0) cmd.Parameters.AddRange(para);

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                cmd.Connection.Close();

                return ds;
            }
        }
        public DataTable ExecuteStoredProcedureDataTable(string procedureName, SqlParameter[] para, CommandType commandType = CommandType.StoredProcedure)
        {
            var ds = ExecuteStoredProcedure(procedureName, para, commandType);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
                return null;
        }

        public List<T> ExecuteStoredProcedure<T>(string procedureName, SqlParameter[] para, CommandType commandType = CommandType.StoredProcedure)
        {
            var ds = ExecuteStoredProcedure(procedureName, para, commandType);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ConvertToList<T>(ds.Tables[0]);
            }
            else
                return null;
        }

        public List<T> ExecuteStoredQuery<T>(string procedureName, SqlParameter[] para)
        {
            var ds = ExecuteStoredProcedure(procedureName, para);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ConvertToList<T>(ds.Tables[0]);
            }
            else
                return null;
        }

        public Object ExecuteScaler(string procedureName, SqlParameter[] para, CommandType commandType = CommandType.StoredProcedure)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[DbConnectionStringKey].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = COMMAND_TIMEOUT;
                cmd.CommandType = commandType;
                cmd.Connection = connection;
                cmd.Connection.Open();

                cmd.CommandText = procedureName;

                if (para != null && para.Length > 0) cmd.Parameters.AddRange(para);

                object value = cmd.ExecuteScalar();

                cmd.Connection.Close();

                return value;
            }
        }

        public int ExecuteNonQuery(string procedureName, SqlParameter[] para, CommandType commandType = CommandType.StoredProcedure)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[DbConnectionStringKey].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = COMMAND_TIMEOUT;
                cmd.CommandType = commandType;
                cmd.Connection = connection;
                cmd.Connection.Open();

                cmd.CommandText = procedureName;

                if (para != null && para.Length > 0) cmd.Parameters.AddRange(para);

                int value = cmd.ExecuteNonQuery();

                cmd.Connection.Close();

                return value;
            }
        }

        public List<T> ConvertToList<T>(DataTable dt)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();
            var objectProperties = typeof(T).GetProperties(flags);
            var targetList = dt.AsEnumerable().Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();

                foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                {
                    properties.SetValue(instanceOfT, dataRow[properties.Name], null);
                }
                return instanceOfT;
            }).ToList();

            return targetList;
        }

        public DataTable ConvertToDataTable<T>(IEnumerable<T> iList)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);


                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach (T iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        #endregion
    }
}
