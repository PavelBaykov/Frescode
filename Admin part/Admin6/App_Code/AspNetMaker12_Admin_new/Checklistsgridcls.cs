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
	// Page class for Checklists
	//
	public class cChecklists_grid<C, S> : cChecklists_grid_base<C, S>
		where C : cConnection, new()
		where S : cAdvancedSecurity, new()
	{

		// TblAddReturnPage
		public string Get_TblAddReturnPage() {
			return ReturnUrl;		
		}

		// TblEditReturnPage
		public string Get_TblEditReturnPage() {
			return ReturnUrl;		
		}

		// Row Inserted event
		public override void Row_Inserted(OrderedDictionary rsold, OrderedDictionary rsnew) {	

		// Insert record
		// NOTE: Modify your SQL here, replace the table name, field name and field values

		string sInsertSql = "INSERT INTO ChecklistItems "+
					 "(Status, DateOfLastChange,ChangedBy_Id,Checklist_Id,ItemTemplate_Id) "+
					 "SELECT 	0 as Status,"+
					 "CURRENT_TIMESTAMP as DateOfLastChange,"+
					 rsnew["ChangedBy_Id"] + " as ChangedBy_Id,"+
					 rsnew["Id"] +" as Checklist_Id,"+
					 "chit.Id as ItemTemplate_Id "+
					 "FROM 	ChecklistItemTemplates chit "+
					 "WHERE chit.Checklist_Id="+rsnew["ChecklistTemplate_Id"]
					 ;
		ew_Execute(sInsertSql);
		}
	}

	// Checklists_grid	
	public static cChecklists_grid<cConnection, cAdvancedSecurity> Checklists_grid {
		get { return (cChecklists_grid<cConnection, cAdvancedSecurity>)ew_PageData["Checklists_grid"]; }
		set { ew_PageData["Checklists_grid"] = value; }
	}
}	
