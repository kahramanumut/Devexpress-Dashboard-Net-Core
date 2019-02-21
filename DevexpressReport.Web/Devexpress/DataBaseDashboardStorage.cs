using DevExpress.DashboardWeb;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using DevexpressReport.Web.BL.Abstract;
using DevexpressReport.Web.BL.Concrete;


namespace DevexpressReport.Web.Devexpress
{
    public class DataBaseDashboardStorage : IEditableDashboardStorage
    {
        string connectionString;


        public DataBaseDashboardStorage(string connectionString)
            : base()
        {
            this.connectionString = connectionString;
        }

        public string AddDashboard(XDocument dashboard, string dashboardName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                MemoryStream stream = new MemoryStream();
                dashboard.Save(stream);
                stream.Position = 0;

                SqlCommand InsertCommand = new SqlCommand(
                    "INSERT INTO _MR_Dashboards (Dashboard, Caption) " +
                    "output INSERTED.ID " +
                    "VALUES (@Dashboard, @Caption)");
                InsertCommand.Parameters.Add("Caption", SqlDbType.NVarChar).Value = dashboardName;
                InsertCommand.Parameters.Add("Dashboard", SqlDbType.VarBinary).Value = stream.ToArray();
                InsertCommand.Connection = connection;
                string ID = InsertCommand.ExecuteScalar().ToString();
                connection.Close();
                return ID;
            }
        }

        public IEnumerable<DashboardInfo> GetAvailableDashboardsInfo()
        {
            List<DashboardInfo> list = new List<DashboardInfo>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand GetCommand = new SqlCommand("SELECT ID, Caption FROM _MR_Dashboards");
                GetCommand.Connection = connection;
                SqlDataReader reader = GetCommand.ExecuteReader();
                while (reader.Read())
                {
                    string ID = reader.GetInt32(0).ToString();
                    string Caption = reader.GetString(1);
                    list.Add(new DashboardInfo() { ID = ID, Name = Caption });
                }
                connection.Close();
            }
            return list;
        }

        public XDocument LoadDashboard(string dashboardID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand GetCommand = new SqlCommand("SELECT  Dashboard FROM _MR_Dashboards WHERE ID=@ID");
                GetCommand.Parameters.Add("ID", SqlDbType.Int).Value = Convert.ToInt32(dashboardID);
                GetCommand.Connection = connection;
                SqlDataReader reader = GetCommand.ExecuteReader();
                reader.Read();
                byte[] data = reader.GetValue(0) as byte[];
                MemoryStream stream = new MemoryStream(data);
                connection.Close();
                return XDocument.Load(stream);
            }
        }

        public void SaveDashboard(string dashboardID, XDocument dashboard)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                MemoryStream stream = new MemoryStream();
                dashboard.Save(stream);
                stream.Position = 0;

                SqlCommand InsertCommand = new SqlCommand(
                    "UPDATE _MR_Dashboards Set Dashboard = @Dashboard " +
                    "WHERE ID = @ID");
                InsertCommand.Parameters.Add("ID", SqlDbType.Int).Value = Convert.ToInt32(dashboardID);
                InsertCommand.Parameters.Add("Dashboard", SqlDbType.VarBinary).Value = stream.ToArray();
                InsertCommand.Connection = connection;
                InsertCommand.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
