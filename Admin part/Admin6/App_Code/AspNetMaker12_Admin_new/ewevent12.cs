using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.WebPages;
using System.Web.Helpers;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Common;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Linq;
using System.Dynamic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using Microsoft.VisualBasic;
using Microsoft.Web.Helpers;
using WebMatrix.Data;
using Newtonsoft.Json;
using System.Data.SqlClient;
using ewConnection = System.Data.SqlClient.SqlConnection;
using ewCommand = System.Data.SqlClient.SqlCommand;
using ewDataReader = System.Data.SqlClient.SqlDataReader;
using ewTransaction = System.Data.SqlClient.SqlTransaction;
using ewDbType = System.Data.SqlDbType;

//
// ASP.NET Maker 12 Project Class
//
public partial class AspNetMaker12_Admin_new : AspNetMaker12_Admin_new_base {

	//
	// Global user code
	//
	//
	// Global events
	//
	// Page Rendering event
	public void Page_Rendering() {

		//ew_Write("Page Rendering");
	}

	//
	// Connection
	//
	public class cConnection : cConnectionBase {

		// Constructor
		public cConnection(string ConnStr) : base(ConnStr)
		{
		}

		// Constructor
		public cConnection() : base()
		{	
		}
	}

	// Execute SQL
	public static int ew_Execute(string Sql)
	{
		using (var c = new cConnection()) { 
			return c.ExecuteNonQuery(Sql);
		}
	}

	// Execute SQL and return first value of first row
	public static object ew_ExecuteScalar(string Sql)
	{
		using (var c = new cConnection()) {
			return c.ExecuteScalar(Sql);
		}
	}

	// Execute SQL and return first value of first row as string
	// for use with As<TValue>, As<TValue>(String, TValue) and Is<TValue>
	public static string ew_ExecuteValue(string Sql)
	{
		using (var c = new cConnection()) {
			return Convert.ToString(c.ExecuteScalar(Sql));
		}
	}

	// Execute SQL and return first row as OrderedDictionary
	public static OrderedDictionary ew_ExecuteRow(string Sql)
	{
		using (var c = new cConnection()) {
			return c.GetRow(Sql);
		}
	}

	// Execute SQL and return List<OrderedDictionary>
	public static List<OrderedDictionary> ew_ExecuteRows(string Sql)
	{
		using (var c = new cConnection()) {
			return c.GetRows(Sql);
		}
	}

	// Executes the query, and returns the row(s) as JSON
	public static string ew_ExecuteJson(string Sql, bool FirstOnly = true)
	{
		using (var c = new cConnection()) {
			if (FirstOnly) {
				var list = new List<OrderedDictionary>();
				list.Add(c.GetRow(Sql));
				return JsonConvert.SerializeObject(list);
			} else {
				return JsonConvert.SerializeObject(c.GetRows(Sql));
			}
		}
	}

	// Execute SQL and return first row
	public static DbDataRecord ew_ExecuteRecord(string Sql)
	{
		using (var c = new cConnection()) {
			return c.GetRecord(Sql);
		}
	}

	// Execute SQL and return List<DbDataRecord>
	public static List<DbDataRecord> ew_ExecuteRecords(string Sql)
	{
		using (var c = new cConnection()) {
			return c.GetRecords(Sql);
		}
	}	

	//
	// Advanced Security
	//
	public class cAdvancedSecurity : cAdvancedSecurityBase {

		public cConnection Conn;

		public cAdvancedSecurity() : base((ew_PageData["Conn"] is cConnection) ? (cConnection)ew_PageData["Conn"] : new cConnection()) {		
		}
	}

	//
	// Menu
	//
	public class cMenu : cMenuBase {

		public cMenu(object MenuId, bool Mobile = false) : base(MenuId, Mobile) {
		}

		public override string Render(bool ret = false) {
			var m = this;
			if (IsRoot)
				Menu_Rendering(ref m);
			return base.Render(ret);
		}

		public void Menu_Rendering(ref cMenu Menu) {

			// Change menu items here
		}
	}	
}	
