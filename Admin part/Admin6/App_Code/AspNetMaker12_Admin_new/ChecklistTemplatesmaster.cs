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
	// Page class for ChecklistTemplates
	//
	public class cChecklistTemplates_master : cChecklistTemplates
	{

		// Page ID
		public string PageID = "master";

		// Project ID
		public string ProjectID = "{6EA87CB0-ED50-4AE1-9743-D14163EABB5A}";

		// Table name
		public string TableName = "ChecklistTemplates";

		// TblAddReturnPage
		public string Get_TblAddReturnPage() {
			return ReturnUrl;		
		}

		// TblEditReturnPage
		public string Get_TblEditReturnPage() {
			return ReturnUrl;		
		}
	}

	// ChecklistTemplates_master	
	public static cChecklistTemplates_master ChecklistTemplates_master {
		get { return (cChecklistTemplates_master)ew_PageData["ChecklistTemplates_master"]; }
		set { ew_PageData["ChecklistTemplates_master"] = value; }
	}
}	
