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

namespace ADUserInformation.wpApproval
{
    public partial class wpApprovalUserControl : UserControl
    {
        

        ListOps objList = new ListOps();

        #region Load Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblvalid.Text = "";
                fetchAppPageInfo();
            }
        }

        private void fetchAppPageInfo()
        {
            SPWeb web = SPContext.Current.Web;
            SPUser user2 = web.CurrentUser;
            string login_name = user2.LoginName;
            int i = login_name.IndexOf("\\");
            string login_psno = Convert.ToString(login_name.Substring(i + 1));

            
            DirectorySearcher mySearcher = null;
            MyActiveDirectory myAD = new MyActiveDirectory();
            mySearcher = myAD.CreateDirectorySearcher(login_psno);

            SearchResult result = mySearcher.FindOne();
            hdAppLoginName.Value = Convert.ToString(myAD.GetProperty(result, "givenname")) + " " + Convert.ToString(myAD.GetProperty(result, "sn"));
                                    
            const string appStatus = "Sent for Approval";
            SPSite site = SPContext.Current.Site;
            using (web = site.OpenWeb())
            {

                SPList applist = web.Lists["Requestee List"];
                List<object> lstResult = new List<object>();

                var selectedEmps = (from SPListItem item in applist.Items
                                    where Convert.ToString(item["IS"]).Substring(Convert.ToString(item["IS"]).IndexOf('#') + 1).Trim() == hdAppLoginName.Value.Trim() && Convert.ToString(item["Status"]).Trim() == appStatus.Trim()
                                    select new
                                    {
                                        ID = item["ID"],
                                        apNames = Convert.ToString(item["Created By"]).Substring(Convert.ToString(item["Created By"]).IndexOf('#') + 1).Trim(),
                                        apModule = new SPFieldLookupValue(Convert.ToString(item["ModuleName"])).LookupValue
                                    }).ToList();
                foreach (var obj in selectedEmps)
                {
                    lstResult.Add(obj);
                }
                grdAppPage.DataSource = lstResult;
                grdAppPage.DataBind();
            }
        }

#endregion 

        #region Page Events

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkAll =
               (CheckBox)grdAppPage.HeaderRow.FindControl("chkSelectAll");
            if (chkAll.Checked == true)
            {
                lblvalid.Text = "";
                foreach (GridViewRow gvRow in grdAppPage.Rows)
                {
                    CheckBox chkSel =
                         (CheckBox)gvRow.FindControl("chkSelect");
                    chkSel.Checked = true;
                }
            }
            else
            {
                foreach (GridViewRow gvRow in grdAppPage.Rows)
                {
                    CheckBox chkSel = (CheckBox)gvRow.FindControl("chkSelect");
                    chkSel.Checked = false;
                }
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            int c = 0;
            foreach (GridViewRow gvRow in grdAppPage.Rows)
            {
                CheckBox chkSel =
                     (CheckBox)gvRow.FindControl("chkSelect");
                if (chkSel.Checked)
                {
                    c++;
                    //lblvalid.Text = "";
                    int intID = Convert.ToInt32(grdAppPage.DataKeys[gvRow.DataItemIndex]["ID"]);
                    objList.Approval(intID);
                    fetchAppPageInfo();
                }
            }
            int rowcnt = grdAppPage.Rows.Count;
            if (rowcnt > 0)
            {
                if (c == 0)
                {
                    showValiLabel();
                }
                else
                    lblvalid.Text = "";
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            int c = 0;
            foreach (GridViewRow gvRow in grdAppPage.Rows)
            {
                CheckBox chkSel =
                     (CheckBox)gvRow.FindControl("chkSelect");
                if (chkSel.Checked)
                {
                    c++;
                    //lblvalid.Text = "";
                    int intID = Convert.ToInt32(grdAppPage.DataKeys[gvRow.DataItemIndex]["ID"]);
                    objList.Rejection(intID);
                    fetchAppPageInfo();
                }
            }
            int rowcnt = grdAppPage.Rows.Count;
            if (rowcnt > 0)
            {
                if (c == 0)
                {
                    showValiLabel();
                }
                else
                lblvalid.Text = "";
            }
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            string strUrl = Convert.ToString(ConfigurationManager.AppSettings["SiteUrl"]);
            Response.Redirect(strUrl, false);
        }

        private void showValiLabel()
        {
            lblvalid.Text = "Kindly select at least 1 item to perform the specified action";

        }

        #endregion

        #region OldCode

        //protected void grdAppPage_Sorting(object sender, GridViewSortEventArgs e)
        //{

        //    Retrieve the table from the session object.
        //    DataTable dt = Session["TaskTable"] as DataTable;

        //    if (dt != null)
        //    {

        //        Sort the data.
        //        dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
        //        grdAppPage.DataSource = Session["TaskTable"];
        //        grdAppPage.DataBind();
        //    }

        //}

        //private string GetSortDirection(string column)
        //{

        //     By default, set the sort direction to ascending.
        //    string sortDirection = "ASC";

        //     Retrieve the last column that was sorted.
        //    string sortExpression = ViewState["SortExpression"] as string;

        //    if (sortExpression != null)
        //    {
        //         Check if the same column is being sorted.
        //         Otherwise, the default value can be returned.
        //        if (sortExpression == column)
        //        {
        //            string lastDirection = ViewState["SortDirection"] as string;
        //            if ((lastDirection != null) && (lastDirection == "ASC"))
        //            {
        //                sortDirection = "DESC";
        //            }
        //        }
        //    }
        //     Save new values in ViewState.
        //    ViewState["SortDirection"] = sortDirection;
        //    ViewState["SortExpression"] = column;

        //    return sortDirection;
        //}

        #endregion
    }
}
