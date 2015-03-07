/*
 * Author: Shahrooz Sabet
 * Date: 20140628
 * */
using System;
using System.Runtime.Serialization;
using System.ServiceModel;
// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
[ServiceContract]
public interface IService
{

	/// <summary>
	/// Gets the data, For testing purpose.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <returns>Echos the value</returns>
	[OperationContract]
	string GetData(int value);

	[OperationContract]
	RefreshDataCT RefreshData(RefreshDataCT composite);

}

// Use a data contract as illustrated in the sample below to add composite types to service operations.

[DataContract]
public class RefreshDataCT
{
	#region WebTableCode Column Definition
	[DataMember]
	public int CompanyCode { get; set; }
	[DataMember]
	public int TableCode { get; set; }
	[DataMember]
	public string TableName { get; set; }
	[DataMember]
	public int TableDataVersion { get; set; }
	[DataMember]
	public string QueryCreateTable { get; set; }
	[DataMember]
	public Int16 IsCreateTmpTable { get; set; }
	[DataMember]
	public string DeleteKey { get; set; }
	[DataMember]
	public string QueryBeforeExec1 { get; set; }// These fields are defined with its table's column for future use and currently are unused.
	[DataMember]
	public string QueryBeforeExec2 { get; set; }
	[DataMember]
	public string QueryAfterExec1 { get; set; }
	[DataMember]
	public string QueryAfterExec2 { get; set; }
	[DataMember]
	public string QueryAfterExec3 { get; set; }
	#endregion

	[DataMember]
	public byte[] DataTableArray { get; set; }

	[DataMember]
	public bool HasCreateTable { get; set; }
	[DataMember]
	public int UserCode { get; set; }
	[DataMember]
	public string DbNameClient { get; set; }
	[DataMember]
	public string DbNameServer { get; set; }
	[DataMember]
	public long Len { get; set; }// For debuging purpose
	[DataMember]
	public string StrDebug { get; set; }// For debuging purpose

}

//DataTable dt;
//MemoryStream mS;

//[DataMember]
//public MemoryStream MS
//{
//    get { return mS; }
//    set { mS = value; }
//}
//[DataMember]
//public DataTable Dt
//{
//    get { return dt; }
//    set { dt = value; }
//}