/*
 * Author: Shahrooz Sabet
 * Date: 20140628
 * */
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// NmdDBAdapter is written for DB communication
/// </summary>
	public static class NmdDBAdapter
	{
		internal static DataTable ExecuteSQL(string StrSql)
		{
			string con = GetConnectionString();
			using (SqlConnection cnInv = new SqlConnection(con))
			{
				cnInv.Open();
				using (SqlCommand cmd = new SqlCommand(StrSql, cnInv))
				using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
				using (DataTable dt = new DataTable())
				{
					dt.TableName = "dtName";
					adapter.Fill(dt);
					dt.RemotingFormat = SerializationFormat.Binary;
					return dt;
				}
			}
		}
		private static string GetConnectionString()
		{
			ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;
			if (settings != null)
				foreach (ConnectionStringSettings cs in settings)
					if (cs.Name == "zWebSrvCN")
						return (cs.ConnectionString);
			return null;
		}
	}
