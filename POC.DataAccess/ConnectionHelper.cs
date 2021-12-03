using System.Configuration;
using System.Data.EntityClient;

namespace POC.DataAccess
{
    public class ConnectionHelper
    {
        public string DbConnectionStringKey
        {
            get
            {
                return ConfigurationManager.AppSettings["DbConnectionStringKey"];
            }
        }
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[DbConnectionStringKey].ConnectionString;
            }
        }
        static string _entityConnection = null;
        public string EntityConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_entityConnection))
                {
                    EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
                    entityBuilder.Provider = "System.Data.SqlClient";
                    entityBuilder.ProviderConnectionString = ConnectionString;
                    entityBuilder.Metadata = @"res://*/POC.csdl|res://*/POc.ssdl|res://*/POC.msl";
                    _entityConnection = entityBuilder.ToString();
                }
                return _entityConnection;
            }
        }
    }
}
