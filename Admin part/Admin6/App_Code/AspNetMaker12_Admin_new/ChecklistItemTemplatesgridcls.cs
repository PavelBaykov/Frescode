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
	// Page class for ChecklistItemTemplates
	//
	public class cChecklistItemTemplates_grid<C, S> : cChecklistItemTemplates_grid_base<C, S>
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

		// Row Inserting event
		public override bool Row_Inserting(OrderedDictionary rsold, ref OrderedDictionary rsnew) {

			// Enter your code here
			// To cancel, set return value to False and error message to CancelMessage

			var value = ew_ExecuteScalar("SELECT max(ordernumber) FROM ChecklistItemTemplates WHERE Checklist_Id = "+Convert.ToInt32(rsnew["Checklist_Id"]));
			if (value.GetType() != typeof(DBNull))
				rsnew["OrderNumber"] = Convert.ToInt32(value)+1;
			else
				rsnew["OrderNumber"] = 1;
			return true;
		}

		// Recordset Deleting event
		public override bool Row_Deleting(OrderedDictionary rs) {

			// Enter your code here
			// To cancel, set return value to False and error message to CancelMessage

				string sDeleteSql = "DELETE FROM ChecklistItems WHERE ItemTemplate_id="+rs["Id"];
			ew_Execute(sDeleteSql);
			return true;
		}
	}

	// ChecklistItemTemplates_grid	
	public static cChecklistItemTemplates_grid<cConnection, cAdvancedSecurity> ChecklistItemTemplates_grid {
		get { return (cChecklistItemTemplates_grid<cConnection, cAdvancedSecurity>)ew_PageData["ChecklistItemTemplates_grid"]; }
		set { ew_PageData["ChecklistItemTemplates_grid"] = value; }
	}
}	
