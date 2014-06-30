using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Data;
using System.Collections.Generic;
using ADUserInformation.Business_Layer;
using System.Configuration;
using System.ComponentModel;
using System.DirectoryServices;
using System.Data.OleDb;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint.WebControls;
using System.Linq;


namespace ADUserInformation.wpUserInfo
{
    public partial class wpUserInfoUserControl : UserControl
    {
        SQLOps objSql = new SQLOps();
        ListOps objList = new ListOps();

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                SPWeb web = SPContext.Current.Web;
                SPUser user = SPContext.Current.Web.AllUsers[this.Page.User.Identity.Name]; // web.CurrentUser;
                //SPUser user2 = SPContext.Current.Web.AllUsers[Page.User.Identity.Name]; // web.CurrentUser;
                string login_name = user.LoginName;


                int i = login_name.IndexOf("\\");
                string domain = Convert.ToString(login_name.Substring(0, i + 1));
                hdLoginName.Value = Convert.ToString(login_name.Substring(i + 1));
                //hdLoginName.Value = "20007577";

                DirectorySearcher mySearcher = null;
                MyActiveDirectory myAD = new MyActiveDirectory();
                mySearcher = myAD.CreateDirectorySearcher(hdLoginName.Value);

                SearchResult result = mySearcher.FindOne();
                lblName.Text = Convert.ToString(myAD.GetProperty(result, "givenname")) + " " + Convert.ToString(myAD.GetProperty(result, "sn"));
                lblDeptCode.Text = Convert.ToString(myAD.GetProperty(result, "department"));
                lblISName.Text = Convert.ToString(myAD.GetProperty(result, "manager"));
                lblLocation.Text = Convert.ToString(myAD.GetProperty(result, "l"));

                int intLocatinID = 0;

                if (lblLocation.Text == Convert.ToString(LocationID.Powai))
                {
                    intLocatinID = LocationID.Powai.GetHashCode();
                }
                else if (lblLocation.Text == Convert.ToString(LocationID.Hazira))
                {
                    intLocatinID = LocationID.Hazira.GetHashCode();
                }
                else if (lblLocation.Text == Convert.ToString(LocationID.Ranoli))
                {
                    intLocatinID = LocationID.Ranoli.GetHashCode();
                }
                else if (lblLocation.Text == Convert.ToString(LocationID.Bangalore))
                {
                    intLocatinID = LocationID.Bangalore.GetHashCode();
                }
                else if (lblLocation.Text == Convert.ToString(LocationID.Coimbatore))
                {
                    intLocatinID = LocationID.Coimbatore.GetHashCode();
                }
                else if (lblLocation.Text == Convert.ToString(LocationID.Talegaon))
                {
                    intLocatinID = LocationID.Talegaon.GetHashCode();
                }


                adInfo.DirectoryService adsbu = new adInfo.DirectoryService();
                lblSbu.Text = adsbu.GetBaanSBUFromUserID(hdLoginName.Value, intLocatinID);

                if (lblSbu.Text == "")
                { lblSbu.Text = " - "; }

                mySearcher = myAD.CreateDirectorySearcher(lblISName.Text);
                result = mySearcher.FindOne();
                hdISPsno.Value = domain + Convert.ToString(myAD.GetProperty(result, "samaccountname"));

                string IsTest = Convert.ToString(ConfigurationManager.AppSettings["IsTest"]);

                DataTable dt = new DataTable();

                if (IsTest == "YES")
                {
                    dt = objSql.getEmpInfo("20007577");
                }
                else
                {
                    dt = objSql.getEmpInfo(hdLoginName.Value);
                }

                if (dt.Rows.Count > 0)
                {
                    //lblName.Text = Convert.ToString(dt.Rows[0]["NAME"]);
                    //lblDeptCode.Text = Convert.ToString(dt.Rows[0]["DEPTCODE"]);
                    //lblLocation.Text = Convert.ToString(dt.Rows[0]["LOCATION"]);
                    //hdISPsno.Value = Convert.ToString(dt.Rows[0]["ISPSNO"]);
                    //lblISName.Text = Convert.ToString(dt.Rows[0]["ISNAME"]);
                    // hdISEmail.Value = Convert.ToString(dt.Rows[0]["ISEMAIL"]);
                }

                fetchFunctionName();
                //fetchSBUName();
                string fnName;
                fnName = Convert.ToString(ddlFunctionList.Items[0].Value);
                fetchModuleName(fnName);

            }
        }

        #endregion

        #region Page Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //string sbu = string.Empty;
            //string modulename = string.Empty;
            string[] modulename = new string[10];
            string strUrl = Convert.ToString(ConfigurationManager.AppSettings["SiteUrl"]);
            int j = 0;
            const string strApproval = "Sent for Approval";
            for (int i = 0; i < lstModuleList.Items.Count; i++)
            {
                if (lstModuleList.Items[i].Selected)
                {
                    modulename[j] = lstModuleList.Items[i].Value;
                    bool blnSuccess = objList.AddInRequestFormLst(lblSbu.Text, lblDeptCode.Text, lblLocation.Text, hdISPsno.Value, modulename[j], strApproval);
                    j++;
                }
               
                //if (ddlSbu.SelectedIndex > 0)
                //{
                //    sbu = ddlSbu.SelectedItem.Text;
                //}
                //else
                //{
                //    sbu = Convert.ToString(ddlSbu.Items[0].Text);
                //}

                //sendEmail();

            }
            if (j==0)
                {
                    lbldisp.Text = "Kindly select at least one module.";
                    return;
                }
            Response.Redirect(strUrl, false);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string strUrl = Convert.ToString(ConfigurationManager.AppSettings["SiteUrl"]);
            Response.Redirect(strUrl, false);
        }

        protected void ddlFunctionList_SelectedIndexChanged(object sender, EventArgs e)
        {

            string fnName;
            if (ddlFunctionList.SelectedIndex > 0)
            {
                fnName = ddlFunctionList.SelectedItem.Value;
            }
            else
            {
                fnName = Convert.ToString(ddlFunctionList.Items[0].Value);
            }
            fetchModuleName(fnName);
        }

        #endregion

        #region Other Methods

        private void sendEmail()
        {
            string emp_name = string.Empty;
            string strPSNo = string.Empty;
            string isname = string.Empty;
            string isemail = string.Empty;
            string strEmail = string.Empty;
            SearchResult result;
            MailSender objEmail = new MailSender();

            DirectorySearcher mySearcher = null;
            MyActiveDirectory myAD = new MyActiveDirectory();
            mySearcher = myAD.CreateDirectorySearcher(lblISName.Text);

            result = mySearcher.FindOne();
            strEmail = myAD.GetProperty(result, "mail");
            #region Send Email Functionality

            try
            {
                string IsTestingMail = Convert.ToString(ConfigurationManager.AppSettings["IsEmailTesting"]).Trim();
                string strTestEmail = Convert.ToString(ConfigurationManager.AppSettings["TestingEmail"]).Trim();
                //string strCcEmails = Convert.ToString(ConfigurationManager.AppSettings["EmailCc"]).Trim();

                string ContactList = string.Empty;
                string strEmailCc = string.Empty;


                if (IsTestingMail == "YES")
                {
                    ContactList = strTestEmail;
                    //strEmailCc = Convert.ToString(strCcEmails).Trim();
                }
                else
                {
                    ContactList = Convert.ToString(isemail).Trim();
                    //strEmailCc = Convert.ToString(strEmail).Trim();
                }

                string strSubject = "Training Approval";
                string strBody = objEmail.GetMailTemplate(); //"Send for Approval for Accounting";                
                string strLink = Convert.ToString(ConfigurationManager.AppSettings["ApprovalPage"]);
                string strMessage = "This is a training request which requires your approval.";
                strBody = strBody.Replace("_msg_", strMessage)
                    .Replace("_ISName_", isname)
                    .Replace("_Link_", strLink)
                    .Replace("_linkmsg_", " to approve.");

                if (!string.IsNullOrEmpty(ContactList))
                {
                    objEmail.SendMail(strSubject, ContactList, strBody, strEmailCc);
                }
            }
            catch (Exception ex)
            {
                // cnLF.WriteToLogFile("ContractNote.buttonOK_Click", " Email Sending for (" + CNID + ") " + ex.Message);
            }

            #endregion

        }

        private enum LocationID
        {
            Powai = 450,
            Hazira = 500,
            Ranoli = 510,
            Bangalore = 470,
            Coimbatore = 480,
            Talegaon = 460
        }

        private void fetchFunctionName()
        {
            SPSite site = SPContext.Current.Site;
            using (SPWeb web = site.OpenWeb())
            {

                SPList fnlist = web.Lists["Module Master"];
                // SPListItemCollection lstCollection = list.Items;

                List<object> lstResult = new List<object>();
                var objModuleMaster = (from SPListItem item in fnlist.Items
                                       select new
                                       {
                                           functionName = item["Function"]
                                       }).ToList().Distinct();
                foreach (var obj in objModuleMaster)
                {
                    lstResult.Add(obj);
                }


                ddlFunctionList.DataSource = lstResult;
                ddlFunctionList.DataValueField = "functionName";
                ddlFunctionList.DataTextField = "functionName";
                ddlFunctionList.DataBind();
                ddlFunctionList.SelectedIndex = 0;

            }
        }

        private void fetchModuleName(string fnName)
        {

            SPSite site = SPContext.Current.Site;
            using (SPWeb web = site.OpenWeb())
            {

                SPList mdlist = web.Lists["Module Master"];
                List<object> lstResult = new List<object>();

                var objModuleMaster = (from SPListItem item in mdlist.Items
                                       where Convert.ToString(item["Function"]).Trim() == fnName.Trim()
                                       select new
                                       {
                                           ID = item["ID"],
                                           moduleName = item["TOPICS"]
                                       }).ToList();
                foreach (var obj in objModuleMaster)
                {
                    lstResult.Add(obj);
                }


                lstModuleList.DataSource = lstResult;
                lstModuleList.DataValueField = "ID";
                lstModuleList.DataTextField = "moduleName";
                lstModuleList.DataBind();
                lstModuleList.SelectedIndex = 0;

            }
        }

        //private void fetchSBUName()
        //{

        //    SPSite site = SPContext.Current.Site;
        //    using (SPWeb web = site.OpenWeb())
        //    {

        //        SPList list = web.Lists["SBU Master"];
        //        SPListItemCollection lstCollection = list.Items;
        //        ddlSbu.DataSource = lstCollection.GetDataTable();

        //        ddlSbu.DataValueField = "SbuID"; // List field holding value - first column is called Title anyway!
        //        ddlSbu.DataTextField = "SbuName"; // List field holding name to be displayed on page 
        //        ddlSbu.DataBind();

        //        ddlSbu.SelectedIndex = 0;
        //    }
        //}

        public static string getPSNOFromLoginName(string LoginName)
        {
            string name = string.Empty;
            try
            {
                name = LoginName.Substring(LoginName.LastIndexOf('\\') + 1, (LoginName.Length - (LoginName.LastIndexOf('\\') + 1)));
            }
            catch (Exception)
            { }
            return name;
        }

        #endregion

        #region Old Code

        //private void fetchModuleName(string fnName)
        //{

        //    using (SPWeb web = SPContext.Current.Web)
        //    {

        //        SPList list = web.Lists["Module Master"];
        //        SPListItemCollection lstCollection = list.Items;
        //        ddlModuleList.DataSource = lstCollection.GetDataTable();

        //        ddlModuleList.DataValueField = "ID"; // List field holding value - first column is called Title anyway!
        //        ddlModuleList.DataTextField = "TOPICS"; // List field holding name to be displayed on page 
        //        ddlModuleList.DataBind();

        //        ddlModuleList.SelectedIndex = 0;



        //        /*                   SPWeb site = SPContext.Current.Web;
        //                           DropDownList cboExtensions = new DropDownList();
        //                           SPList list = site.Lists["ImageExtensionList"];
        //                           SPListItemCollection lstCollection = list.Items;
        //                           cboExtensions.DataSource = lstCollection.GetDataTable();
        //                           cboExtensions.DataValueField = "Extension"; // List field holding value
        //                           cboExtensions.DataTextField = "Extension"; // List field holding name to be displayed on page
        //                           cboExtensions.DataBind();
        //                           this.Controls.Add(cboExtensions);

        //                       */
        //    }
        //}

        #endregion
    }
}
