/*
 * Author: Shahrooz Sabet
 * Date: 20140628
 * */
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web.Security;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
[AspNetCompatibilityRequirements(RequirementsMode =
AspNetCompatibilityRequirementsMode.Required)]
public class Service : IService
{
	private const string TAG = "NmdMobileWCFService.Service";
	public string GetData(int value)
	{
		Console.WriteLine(ServiceSecurityContext.Current.PrimaryIdentity.Name);
		if (Roles.IsUserInRole("Enterprise"))
		{
			return string.Format("You entered: {0}", value);
		}
		else return "not authorized";
	}
	[PrincipalPermission(SecurityAction.Demand, Role = "Enterprise")]
	public RefreshDataCT RefreshData(RefreshDataCT composite)
	{
		if (composite == null)
			throw new ArgumentNullException("RefreshDataCT Is Null");
		else if (string.IsNullOrWhiteSpace(composite.TableCode.ToString()))
			throw new ArgumentException("TableCode IsNullOrWhiteSpace");
		try
		{
			SetDbName(composite);
			GetDataRefresh(composite);

		}
		catch (Exception eDB)
		{
			Console.WriteLine(eDB);
			Debug.WriteLine(eDB, TAG);
		}
		return composite;
	}

	private void SetDbName(RefreshDataCT composite)
	{
		string StrSQL = "";
		StrSQL = "Select * From WebActions Where CompanyCode=" + composite.CompanyCode + " And ActionCode=1";
		//composite.StrDebug=GetConnectionString();
		using (DataTable dtWebTableCode = NmdDBAdapter.ExecuteSQL(StrSQL))
		{
			composite.DbNameServer = ((string)dtWebTableCode.Rows[0]["DbNameServer"]).Trim();
			composite.DbNameClient = ((string)dtWebTableCode.Rows[0]["DbNameClient"]).Trim();
		}
	}

	private void GetDataRefresh(RefreshDataCT composite)
	{
		string StrSQL = "";
		string DbNameServer = "";
		string DbNameClient = "";

		if (composite.TableCode > 1000)
		{
			DbNameServer = composite.DbNameServer + ".dbo.";
			DbNameClient = composite.DbNameClient + ".dbo.";
		}

		StrSQL = "Select * From " + DbNameServer + "WebTableCode Where CompanyCode=" + composite.CompanyCode + " And TableCode=" + composite.TableCode;
		//composite.StrDebug=GetConnectionString();
		using (DataTable dtWebTableCode = NmdDBAdapter.ExecuteSQL(StrSQL))
		{
			composite.TableName = ((string)dtWebTableCode.Rows[0]["TableName"]).Trim();
			composite.HasCreateTable = false;
			if ((int)dtWebTableCode.Rows[0]["TableDataVersion"] > composite.TableDataVersion)
			{
				composite.TableDataVersion = 0;
				composite.HasCreateTable = true;
			}
			composite.QueryCreateTable = ((string)dtWebTableCode.Rows[0]["QueryCreateTable"]).Trim();
			composite.QueryBeforeExec1 = ((string)dtWebTableCode.Rows[0]["QueryBeforeExec1"]).Trim();
			composite.QueryBeforeExec2 = ((string)dtWebTableCode.Rows[0]["QueryBeforeExec2"]).Trim();

			composite.QueryAfterExec1 = ((string)dtWebTableCode.Rows[0]["QueryAfterExec1"]).Trim();
			composite.QueryAfterExec2 = ((string)dtWebTableCode.Rows[0]["QueryAfterExec2"]).Trim();
			composite.QueryAfterExec3 = ((string)dtWebTableCode.Rows[0]["QueryAfterExec3"]).Trim();

			composite.DeleteKey = ((string)dtWebTableCode.Rows[0]["DeleteKey"]).Trim();
			composite.IsCreateTmpTable = (Int16)dtWebTableCode.Rows[0]["IsCreateTmpTable"];

			StrSQL = ((string)dtWebTableCode.Rows[0]["QueryGetData"]).Replace("@TableDataVersion", composite.TableDataVersion.ToString());
			if (composite.TableCode > 1000)
				StrSQL = ((string)StrSQL).Replace("@DbNameServer.dbo.", DbNameClient);
			else
				StrSQL = ((string)StrSQL).Replace("@DbNameServer.dbo.", DbNameServer);

		}

		//composite.Dt = ExecuteSQL(StrSQL);
		//composite.MS = ToMemStream(ExecuteSQL(StrSQL));
		composite.DataTableArray = ToMemStream(NmdDBAdapter.ExecuteSQL(StrSQL)).ToArray();

	}
	private MemoryStream ToMemStream(DataTable dt)
	{
		using (MemoryStream dtStream = new MemoryStream())
		{
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(dtStream, dt);
			dtStream.Flush();
			dtStream.Seek(0, SeekOrigin.Begin);
			return dtStream;
		}
	}
}


//private static void DbConn()
//{
//        SqlConnectionStringBuilder builder =
//            new SqlConnectionStringBuilder(GetConnectionString());
//        builder.Password = "2222";
//}
//private string FillSysInfo(int SystemGroup)
//{

//    ////string StrSQL= "Select * From WebSystems Where SystemGroup = "+ SystemGroup ;
//    using (DataSet1TableAdapters.WebSystemsTableAdapter WebSysTA = new DataSet1TableAdapters.WebSystemsTableAdapter())
//    using (DataSet1.WebSystemsDataTable WebSysTbl = WebSysTA.GetData(SystemGroup))
//    {
//        StrName = (String)WebSysTbl.Rows[0][WebSysTbl.SystemSourceColumn];
//        StrConn = (String)WebSysTbl.Rows[0][WebSysTbl.ConnectionColumn];
//    }

//    return StrName;
//}



//ConnectionStringSettingsCollection settings =
//ConfigurationManager.ConnectionStrings;

//if (settings != null)
//{
//    foreach(ConnectionStringSettings cs in settings)
//    {
//        if (cs.Name == "NamaadDBConStrName")
//        {
//            return (cs.ConnectionString);
//        }
//    }
//}

//StrSQL = "DROP TABLE IF EXISTS InvGood; \n " +
//               "CREATE TABLE InvGood( \n " +
//               "ItemCode [char](15) PRIMARY KEY NOT NULL, \n " +
//               "FarsiDesc [varchar](100) NOT NULL, \n " +
//               "LatinDesc [varchar](100) NOT NULL, \n " +
//               "CurrentUnit [smallint] NOT NULL, \n " +
//               "SecondUnit [smallint] NOT NULL, \n " +
//               "CurRScn [smallint] NOT NULL, \n " +
//               "ItemCodeBarcode [char](15) NOT NULL, \n " +
//               "TableDataVersion  [integer] NOT Null  );";


//composite.StrCreateTable = StrSQL;


//switch (composite.TableCode)
//{
//    case 2001:
//        RefreshData2001(composite);
//        break;
//    default:
//        throw new ArgumentException("composite");
//}